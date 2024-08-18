using System.Buffers.Text;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace SonicServer
{
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private static bool _isHandShakeDone = false;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
        }
         public NetworkStream Stream => _stream;

        public void HandleClient()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            StringBuilder messageBuilder = new StringBuilder();
            int contentLength = -1;



            while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) != 0)
            {

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
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"RECEIVED: {completeMessage}");
                        JsonData info = JsonConvert.DeserializeObject<JsonData>(completeMessage)!;
                        JsonData info2 =
                            JsonConvert.DeserializeObject<JsonData>(
                                Encoding.UTF8.GetString(Convert.FromBase64String(info.Payload)))!;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(info2.Payload)));
                        Console.ResetColor();
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
                    if (int.TryParse(line.Substring("Content-Length:".Length).Trim(), out int contentLength))
                    {
                        return contentLength;
                    }
                }
            }
            return -1; // Return -1 if Content-Length header is not found
        }
         private static void HandShake(string message, NetworkStream stream)
        {
            JsonData response;
            switch (message)
            {
                case { } msg when msg.Contains("CMD hi"):
                    Console.WriteLine("we have handshake p1 sending hi back");
                    Send(stream, "RESP", "hi", null, null);
                    break;
                case { } msg when msg.Contains("CMD capabilities"):
                    Console.WriteLine("we have handshake p2 sending capabilites");
                    SendJson(stream, "RESP", "capabilities", null);
                    break;
                case { } msg when msg.Contains("\"For\":\"devicetype\""):
                    Console.WriteLine("sending device info");
                    response = new JsonData
                    {
                        For = "devicetype",
                        Payload = "",
                        Type = "RESP"
                    };
                    SendJson(stream, "DATA", null, response);
                    break;
                case { } msg when msg.Contains("\"For\":\"stalllogin\""):
                    Console.WriteLine("sending stall info");
                    response = new JsonData
                    {
                        For = "stalllogin",
                        Payload = "",
                        Type = "RESP"
                    };
                    SendJson(stream, "DATA", null, response);
                    Console.WriteLine("Handshake complete! ");
                    _isHandShakeDone = true;
                    break;
            }
        }

        public static void Send(NetworkStream conn, string? method, string? path, Dictionary<string, string>? headers, string? body)
    {
        StringBuilder payload = new StringBuilder();
        payload.Append($"{method ?? ""} {path ?? ""}".Trim());
        payload.Append("\r\n");
        if (headers != null)
        {
            foreach (var header in headers)
            {
                payload.Append($"{header.Key}: {header.Value}\r\n");
            }
        }
        payload.Append("\r\n");
        if (!string.IsNullOrEmpty(body))
        {
            payload.Append(body.Replace("\r\n", "\n").Replace("\n", "\r\n").Trim());
        }
        payload.Append("\r\n");
        var buffer = Encoding.UTF8.GetBytes(payload.ToString());
        Console.WriteLine(payload.ToString());
        conn.Write(buffer, 0, buffer.Length);
    }

    public static void SendJson(NetworkStream conn, string method, string? path, object? obj)
    {
        var headers = new Dictionary<string, string>
        {
            { "Content-Type", "text/json" }
        };
        var body = obj != null ? JsonConvert.SerializeObject(obj) : "{}";
        headers["Content-Length"] = Encoding.UTF8.GetByteCount(body).ToString();
        Send(conn, method, path, headers, body);
    }
    public static string B64Json(object data)
    {
        string jsonString = JsonConvert.SerializeObject(data);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
        return Convert.ToBase64String(bytes);
    }

    public static void RetailEvent(NetworkStream conn, string verb, string resource, dynamic body)
    {
        var payload = new
        {
            Type = "DATA",
            For = "stall",
            Payload = B64Json(new
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

       
    }
}