using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace SonicServer
{


    class Program
    {
        private static StringBuilder _stringBuilder = new(); // incase we need it
        private static bool _ifIncomplete = false;
        private static int _expectedContentLength = 0;
        static void Main(string[] args)
        {
            StartServer();
        }

        static void StartServer()
        {
            string host = "127.0.0.1"; // localhost
            int port = 8005; // choose any available port

            // Create a TCP listener
            TcpListener server = new TcpListener(IPAddress.Parse(host), port);

             

            // Start listening for incoming connections
            server.Start();
            Console.WriteLine($"Server listening on {host}:{port}");

            while (true)
            {
                // Accept a connection from a client
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine($"Connection from {((IPEndPoint)client.Client.RemoteEndPoint!).Address}");

                // Handle the client request
                HandleClient(client);
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                // Process the received data
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                
                
                string? response = message switch
                {
                    { } msg when msg.Contains("CMD hi") => "RESP hi\r\n\r\n", // RESP hi
                    { } msg when msg.Contains("CMD capabilities") =>
                        "RESP capabilities\r\nContent-Length: 2\r\nContent-Type: text/json\r\n\r\n{}\n", // RESP capabilities
                    { } msg when msg.Contains("\"For\":\"devicetype\"") =>
                        "DATA \r\nContent-Length: 47\r\nContent-Type: text/json\r\n\r\n{\"For\":\"devicetype\",\"Payload\":\"\",\"Type\":\"RESP\"}", // RESP devicetype
                    { } msg when msg.Contains("\"For\":\"stalllogin\"") =>
                        "DATA \r\nContent-Length: 47\r\nContent-Type: text/json\r\n\r\n{\"For\":\"stalllogin\",\"Payload\":\"\",\"Type\":\"RESP\"}", // RESP stalllogin
                    _ => null
                };
                // just assume in this situation that the data is now json so 
                if (response == null)
                {
                    try
                    {
                        Console.WriteLine($"received payload: {GetJson(message).ReceivedPayload}");
                    }
                    catch (Exception e)
                    {
                        if (e.ToString().Contains("Incomplete JSON payload"))
                        {
                            Console.WriteLine("Payload is not ready yet!");
                        }
                    }
                    
                }
                if (response != null)
                {
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine($"Sent: {Encoding.UTF8.GetString(responseBytes)}");
                }
            }

            // Close the connection with the client
            client.Close();
        }

        public static InitalData GetJson(string data)
        {
            int actualLength;
            string payload;
            Console.WriteLine(data);

            if (!_ifIncomplete)
            {
                var contentLengthMatch = Regex.Match(data, @"Content-Length: (\d+)");
                if (!contentLengthMatch.Success)
                {
                    Console.WriteLine("Content-Length not found.");
                    throw new Exception("Content-Length not found.");
                }

                _expectedContentLength = int.Parse(contentLengthMatch.Groups[1].Value);

                // Extract JSON payload
                var jsonPayloadMatch = Regex.Match(data, @"\r?\n\r?\n(.*)", RegexOptions.Singleline);
                if (!jsonPayloadMatch.Success)
                {
                    Console.WriteLine("JSON payload not found.");
                    throw new Exception("JSON payload not found.");
                }

                payload = jsonPayloadMatch.Groups[1].Value;
                actualLength = payload.Length;
            }
            else
            {
                actualLength = data.Length;
                payload = data;
            }

            // Combine in this situation
            _stringBuilder.Append(payload);
            Console.WriteLine(_stringBuilder.Length);
            Console.WriteLine(_expectedContentLength);
            // *hopefully* we never get to state where this runs away
            if (_stringBuilder.Length == _expectedContentLength)
            {
                Console.WriteLine("its right!");
                payload = _stringBuilder.ToString();
                var outPayload = JsonConvert.DeserializeObject<InitalData>(payload);
                _stringBuilder.Clear();
                _ifIncomplete = false;
                _expectedContentLength = 0;
                Console.WriteLine(_expectedContentLength);
                return outPayload;
            }

            _ifIncomplete = true;
            throw new Exception("Incomplete JSON payload");
        }


    }

    class InitalData
    {
        [JsonProperty("For")] public string ReceivedFor { get; set; } = null!;
        [JsonProperty("Payload")] public string ReceivedPayload { get; set; } = null!;
        [JsonProperty("Type")] public string ReceivedType { get; set; } = null!;
    }
}