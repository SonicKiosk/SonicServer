using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Threading;

namespace SonicServer
{
	public class Server
	{
		private static ILogger ServerLogger = null;

		public static List<ClientHandler> _activeConnections = new List<ClientHandler>();
		public static Settings? settings { get; private set; }
		private static Thread keeper = null;
		public static void Start()
		{
			using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
			ServerLogger = loggerFactory.CreateLogger("Server");
			ServerLogger.LogDebug("working");
			// Automatic host IP address retrival
			IPAddress address = Array.Find(
				Dns.GetHostEntry(Dns.GetHostName()).AddressList,
				(x) => x.AddressFamily == AddressFamily.InterNetwork
			);

			ServerLogger.LogInformation("Reading config from cfg.json");
			settings = Config.ReadFromFile();

			//if (settings.DebugIP != null)
			//{
			//	ServerLogger.Debug($"IP override found. Using ip {settings.DebugIP} (\"DebugIP\")");

			//}
			if (settings.DebugIP == null) // yucky cluttered block
			{
				ServerLogger.LogInformation("Host IP was null. Using Defaults");
			}

			string host = settings.DebugIP ?? "127.0.0.1";
			int port = 8005; // choose any available port
							 // Create a TCP listener
			ServerLogger.LogInformation($"Attempting to create server on {host}:{port}.");
			TcpListener server = null;
			try
			{
				server = new(IPAddress.Parse(host), port);
				server.Start();
			}
			catch (Exception ex)
			{
				ServerLogger.LogError($"Error starting server. Fatal. Message: {ex.Message}");
				ServerLogger.LogError("Quitting.");
				Environment.Exit(1);
			}

			if (server == null)
			{
				ServerLogger.LogError($"Server was null. Quitting.");
				Environment.Exit(1);
			}
			// Start listening for incoming connections
			ServerLogger.LogInformation($"Server listening on {host}:{port}");
			keeper = new Thread(() => {
				while (true)
				{
					// TODO: make not throw
					// Accept a connection from a client
					try
					{
						TcpClient client = server.AcceptTcpClient();
						if (client != null && client.Connected)
						{
							ServerLogger.LogInformation($"Connection from {((IPEndPoint)client.Client.RemoteEndPoint!).Address}");
							// Handle the client request using ClientHandler
							_activeConnections.Add(new ClientHandler(client));
						}
					}
					catch (Exception ex)
					{
						ServerLogger.LogError($"Error accepting client. Fatal. Message: {ex.Message}");
						ServerLogger.LogError("Quitting.");
						Environment.Exit(1);
					}
				}
			});
			
		}
		public static void HandleDisconnect(ClientHandler handler)
		{
			ServerLogger.LogInformation("Client", handler.id, "requested disconnect.");
			if (_activeConnections.Remove(handler))
				ServerLogger.LogInformation("Successfully removed client", handler.id, "from registry.");
			else
				ServerLogger.LogError("Failed to remove client", handler.id, "from registry.");
		}
	}
}
