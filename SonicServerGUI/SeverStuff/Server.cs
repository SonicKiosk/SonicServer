using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SonicServer
{
	public class Server
	{
		public static List<ClientHandler> _activeConnections;
		static void Start()
		{
			// Automatic host IP address retrival
			IPAddress address = Array.Find(
				Dns.GetHostEntry(Dns.GetHostName()).AddressList,
				(x) => x.AddressFamily == AddressFamily.InterNetwork
			);

			string host = address.ToString(); //"192.168.1.109"; // localhost
											  //! address = "192.168.1.109"
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
				Console.WriteLine($"Connection from {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
				// Handle the client request using ClientHandler
				_activeConnections.Add(new ClientHandler(client));
			}
		}
	}
}
