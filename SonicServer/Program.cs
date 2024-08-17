using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SonicServer
{


    class Program
    {
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
                Console.WriteLine($"Received: {message}");
                
                string? response = message switch
                {
                    { } msg when msg.Contains("CMD hi") => "524553502068690d0a0d0a", // RESP hi
                    { } msg when msg.Contains("CMD capabilities") =>
                        "52455350206361706162696c69746965730d0a436f6e74656e742d4c656e6774683a20320d0a436f6e74656e742d547970653a20746578742f6a736f6e0d0a0d0a7b7d0a", // RESP capabilities
                    { } msg when msg.Contains("\"For\":\"devicetype\"") =>
                        "44415441200d0a436f6e74656e742d4c656e6774683a2034370d0a436f6e74656e742d547970653a20746578742f6a736f6e0d0a0d0a7b22466f72223a2264657669636574797065222c225061796c6f6164223a22222c2254797065223a2252455350227d", // RESP devicetype
                    { } msg when msg.Contains("\"For\":\"stalllogin\"") =>
                        "44415441200d0a436f6e74656e742d4c656e6774683a2034370d0a436f6e74656e742d547970653a20746578742f6a736f6e0d0a0d0a7b22466f72223a227374616C6C6C6F67696E222c225061796c6f6164223a22222c2254797065223a2252455350227d", // RESP stalllogin
                    _ => null
                };

                if (response != null)
                {
                    byte[] responseBytes = StringToByteArray(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine($"Sent: {Encoding.UTF8.GetString(responseBytes)}");
                }
            }

            // Close the connection with the client
            client.Close();
        }

        static byte[] StringToByteArray(string? hex)
        {
            int numberChars = hex!.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}