using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pastel;
using SonicServer.JsonClasses;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;

namespace SonicServer
{
    public class ClientHandler
    {
        Logger ClientLogger;
        public CustomerInfo clientInfo;
        private const int ClientBufferSize = 2048; // doubled the size since packets can be large asl
        private NetworkStream _stream;
        private readonly TcpClient client;
        private bool _isHandShakeDone = false;
        public Guid id { get; private set; } = Guid.Empty;
        private Dictionary<string, Entry> _existingItems = new Dictionary<string, Entry>();

        public ClientHandler(TcpClient client)
        {
            this.client = client;
            this.id = Guid.NewGuid();
            //{((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString().Pastel(Color.MediumAquamarine)}
            ClientLogger = new Logger($"Client {id.ToString().Pastel(Color.MediumAquamarine)}", Color.Chartreuse);

            //ClientLogger.Info("herro");
            new Thread(() =>
            {
                _stream = client.GetStream();
                ClientLogger.Info("Initialized ClientHandler.");
                HandleClient();
            }).Start();

            new Thread(CheckForDisconnect).Start();
        }
        public NetworkStream Stream => _stream;
        private bool _isDisconnecting = false;
        public void Disconnect()
        {
            if (_isDisconnecting)
                return;
            _isDisconnecting = true;
            ClientLogger.Info("Disconnecting.");
            SServer.HandleDisconnect(this);
        }
        public void CheckForDisconnect()
        {
            while (client.Connected && client.Client.Connected)
                continue;

            Disconnect();
        }
        public void HandleClient()
        {
            byte[] buffer = new byte[ClientBufferSize];
            int bytesRead;
            StringBuilder messageBuilder = new();
            int contentLength = -1;

            while (true)
            {
                if (_stream == null || !_stream.CanRead || !_stream.Socket.Connected)
                {
                    ClientLogger.Info("Stream is dead.");
                    Disconnect();
                    return;
                }
                if ((bytesRead = _stream.Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None, out SocketError errorCode)) == 0)
                    continue;

                if (errorCode != SocketError.Success)
                {
                    ClientLogger.Info("Socket failed.");
                    Disconnect();
                    return;
                }

                string messagePart = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                if (!_isHandShakeDone)
                {
                    HandShake(messagePart, _stream);
                }
                else
                {
                    // Handle other messages after handshake
                    messageBuilder.Append(messagePart);
                    if (contentLength == -1)
                    {
                        // Check if we have received the headers
                        string headers = messageBuilder.ToString();
                        int headerEndIndex = headers.IndexOf("\r\n\r\n");
                        if (headerEndIndex != -1)
                        {
                            // Extract headers
                            string headerPart = headers.Substring(0, headerEndIndex);
                            // Extract content length
                            contentLength = GetContentLength(headerPart);
                            // Remove headers from the message builder
                            messageBuilder.Remove(0, headerEndIndex + 4);
                        }
                    }

                    // Process the received data
                    if (contentLength != -1 && messageBuilder.Length >= contentLength)
                    {
                        string completeMessage = messageBuilder.ToString(0, contentLength);
                        //Console.ForegroundColor = ConsoleColor.DarkRed;
                        //ClientLogger.Info("Received message.");
                        //ClientLogger.Debug($"Recieved Message: {completeMessage.Pastel(Color.DarkRed)}");
                        JsonData info = JsonConvert.DeserializeObject<JsonData>(completeMessage)!;
                        JObject decoded =
                                JsonConvert.DeserializeObject<JObject>(
                                    Encoding.UTF8.GetString(
                                        Convert.FromBase64String(
                                            JsonConvert.DeserializeObject<JsonData>(
                                                Encoding.UTF8.GetString(
                                                    Convert.FromBase64String(info.Payload) // fucking hell this is so stupppid
                                                )
                                            )!.Payload
                                        )
                                    )
                                )!;
                        //Console.ForegroundColor = ConsoleColor.Green;
                        if (decoded.ContainsKey("For") && decoded["For"]!.ToString() == "/sync/snapshot")
                            try
                            {
                                ClientLogger.Debug("syncinfo:", JsonConvert.SerializeObject(JsonConvert.DeserializeObject<SyncInfo>(decoded["SyncInfo"]!.ToString()).Software.CurrentConfig.App).ToString());
                            }
                            catch
                            {
                                ClientLogger.Warn("Failed to parse SyncInfo. Some features may not work.");
                                //Disconnect();
                            }
                                //if (_isHandShakeDone)
                        //{
                        //}
                        // Encoding.UTF8.GetString(Convert.FromBase64String(decoded.Payload)).Pastel(Color.DarkSalmon));
                        //Console.ResetColor();
                        // Clear the message builder for the next message
                        messageBuilder.Clear();
                        contentLength = -1;
                    }
                }
            }
        }

        private int GetContentLength(string headers)
        {
            foreach (string line in headers.Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                if (line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(line["Content-Length:".Length..].Trim(), out int contentLength))
                    {
                        return contentLength;
                    }
                }
            }
            return -1; // Return -1 if Content-Length header is not found
        }
        private void HandShake(string message, NetworkStream stream)
        {
            JsonData response;
            switch (message)
            {
                case { } msg when msg.Contains("CMD hi"): // init, hi
                    ClientLogger.Info("Sending HI");
                    Send(stream, "RESP", "hi", null, null);
                    break;
                case { } msg when msg.Contains("CMD capabilities"): // init 2, capabilities
                    ClientLogger.Info("Sending Capabilities");
                    SendJson(stream, "RESP", "capabilities", null);
                    break;
                case { } msg when msg.Contains("\"For\":\"devicetype\""): // init 3, send device info
                    ClientLogger.Info("Sending device info");
                    response = new JsonData
                    {
                        For = "devicetype",
                        Payload = "",
                        Type = "RESP"
                    };
                    SendJson(stream, "DATA", null, response);
                    break;
                case { } msg when msg.Contains("\"For\":\"stalllogin\""): // init 3, send device (stall) info
                    ClientLogger.Info("Sending stall info");
                    response = new JsonData
                    {
                        For = "stalllogin",
                        Payload = "",
                        Type = "RESP"
                    };
                    SendJson(stream, "DATA", null, response);
                    //RetailEventUtils.Checkin(this, new Customer()
                    //{
                    //    CustomerInfo = new CustomerInfo()
                    //    {
                    //        ID = "joee",
                    //        FirstName = "Joe",
                    //        LastName = "Swanson",
                    //        //Message = "Pluh",
                    //        ProfilePictureUrl = "https://vignette.wikia.nocookie.net/universe-of-smash-bros-lawl/images/7/77/Joe.png/revision/latest?cb=20190422095447"
                    //    }
                    //});
                    //RetailEventUtils.Ticket(this, RetailEventUtils.GetDummyTicket());
                    //AddItem("1", "2", "3", "4", 0, "5");
                    ClientLogger.Info("Handshake complete!");
                    _isHandShakeDone = true;
                    break;
            }
        }
        public void Send(NetworkStream conn, string? method, string? path, Dictionary<string, string>? headers, string? body)
        {
            StringBuilder payload = new StringBuilder($"{method?.Trim() ?? ""} {path?.Trim() ?? ""}\r\n");

            if (headers != null)
                foreach (KeyValuePair<string, string> header in headers)
                    payload.Append($"{header.Key}: {header.Value}\r\n");

            payload.Append("\r\n");
            if (!string.IsNullOrWhiteSpace(body))
                payload.Append(body.Trim().Replace('\r', '\0').Replace("\n", "\r\n"));

            payload.Append("\r\n");


            //string payload = $"{method?.Trim() ?? ""} {path?.Trim() ?? ""}\r\n";
            //if (headers != null)
            //{
            //    foreach (var header in headers)
            //    {
            //        payload += header.Key + ": " + header.Value + "\r\n"; // I know that it looks worse than stringbuilder but it's faster
            //    }
            //}
            //payload += "\r\n";
            //if (!string.IsNullOrEmpty(body))
            //{
            //    payload += body.Trim().Replace('\r', '\0').Replace("\n", "\r\n");
            //}
            //payload += "\r\n";

            byte[] buffer = Encoding.UTF8.GetBytes(payload.ToString());
            //ClientLogger.Debug(payload);
            conn.Socket.Send(buffer, 0, buffer.Length, SocketFlags.None, out SocketError err);
            if (err != SocketError.Success)
                ClientLogger.Error($"(Non-Fatal) Failed to send {buffer.Length} bytes with error {err.ToString()}");
        }

        public void SendJson(NetworkStream conn, string method, string? path, object? obj)
        {
            Dictionary<string, string> headers = new() {
                { "Content-Type", "text/json" }
            };
            string? body = obj != null ? JsonConvert.SerializeObject(obj) : "{}";
            //ClientLogger.Debug(body);
            headers["Content-Length"] = Encoding.UTF8.GetByteCount(body).ToString();
            Send(conn, method, path, headers, body);
        }
        public string B64Json(object data)
        {
            string jsonString = JsonConvert.SerializeObject(data);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
            return Convert.ToBase64String(bytes);
        }
        public void RetailEvent(NetworkStream conn, string verb, string resource, dynamic body)
        {
            JsonData payload = new()
            {
                Type = "DATA",
                For = "stall",
                Payload = B64Json(new JsonData
                {
                    Type = "DATA",
                    For = "stall",
                    ServiceId = "retail",
                    Payload = B64Json(new RetailEventRequest
                    {
                        Type = "RQST",
                        For = "retail",
                        Verb = verb,
                        Resource = resource,
                        PayloadRetail = body
                    })
                })
            };
            SendJson(conn, "DATA", "", payload);
        }
        public static double CalculateTax(double input, int taxPercent=12, int decimals=2) // provincial sales tax (7%) + some federal thing (5%) = 12% tax (for BC, Canada)
        {
            return Math.Round(input * (taxPercent / 100.0), decimals);
        }

        public void AddItem(string itemId, string desc, string category, string price, int quantity, string imagePath)
        {
            int total;
            if (_existingItems.TryGetValue(itemId, out var item))
            {
                // c# this is dumb as shit please fix it rn thx xx // not anymore i made it actually use the correct type lmao
                item.Quantity += 1;
                _existingItems[itemId] = item; // whats that kids? thats right! thats fucking stupid!
                ClientLogger.TestStyles("new quantity:", item.Quantity);
            }
            else
            {
                _existingItems.Add(itemId,
                    new Entry()
                    {
                        Category = category,
                        //ImagePath = imagePath,
                        ItemId = itemId,
                        MktgDescription = desc,
                        ModifierList = new List<Modifier>(),
                        Price = price,
                        Quantity = quantity
                    });// [desc, category, price, quantity, imagePath,]);
            }

            //List<SubTicket> items = new List<Entry>();
            //foreach(Entry entry in _existingItems.Values) {

            float pricesum = _existingItems.Values.ToList().Sum(x => float.Parse(x.Price) * x.Quantity);
            var retailEventRequest = new RetailEventRequest
            {
                Type = "RQST",
                For = "retail",
                Verb = "POST",
                Resource = "/retail/ticket",
                PayloadRetail = new PayloadRetail
                {
                    Ticket = new Ticket
                    {
                        State = "ACTIVE",
                        Total = pricesum.ToString(),
                        Tax = CalculateTax(pricesum).ToString(),
                        EmployeeFirstName = "Silly",
                        EmployeeLastName = "Billy",
                        SubTicketList = new List<SubTicket>{
                            new() {
                                EntryList = _existingItems.Values.ToList()/*new List<Entry>{
                                    new() {
                                        ItemId = "1002",
                                        MktgDescription = "COCK",
                                        Category = "COCK",
                                        Price = "69",
                                        Quantity = 69,
                                        ImagePath = "../../../../../../../test.png",
                                        ModifierList = new List<Modifier>{ new()
                                        {
                                            ModifierId = "morecock",
                                            MktgDescription = "more cock"
                                        }}
                                    }
                                }*/
                            }
                        }
                    }
                }
            };

            RetailEvent(Stream, retailEventRequest.Verb, retailEventRequest.Resource, retailEventRequest.PayloadRetail);
        }
    }
}