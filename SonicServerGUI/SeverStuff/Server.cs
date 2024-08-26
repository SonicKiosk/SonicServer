using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SonicServer
{
	public class Server
	{
		private static ILogger ServerLogger = null;

		public static List<ClientHandler> ActiveConnections = new List<ClientHandler>();
		public static Settings settings { get; private set; }
		private static Thread keeper = null;
		private static TcpListener server = null;
		public static bool IsStarted { get; private set; }
		public static void Start(IPAddress address, int port)
		{
			using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
			ServerLogger = loggerFactory.CreateLogger("Server");
			ServerLogger.LogDebug("working");
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
			ServerLogger.LogInformation($"Attempting to create server on {address}:{port}.");
			try
			{
				server = new(/*address*/ IPAddress.Parse("0.0.0.0"), port);
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
			else
			{
				IsStarted = true;
			}
			// Start listening for incoming connections
			ServerLogger.LogInformation($"Server listening on {address}:{port}");
			keeper = new Thread(AcceptConnections);
			keeper.Start();
		}
		private static void AcceptConnections()
		{
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
						ActiveConnections.Add(new ClientHandler(client));
					}
				}
				catch (Exception ex)
				{
					ServerLogger.LogError($"Error accepting client. Fatal. Message: {ex.Message}");
					ServerLogger.LogError("Quitting.");
					Environment.Exit(1);
				}
			}
		}
		public static void Stop()
		{
			if (!IsStarted)
			{
				return;
			}
			keeper.Abort();
			ServerLogger.LogInformation("Stopping Server");
			foreach (ClientHandler item in ActiveConnections)
			{
				item.Disconnect();
			}
			server.Stop();
			server = null;
			keeper = null;
			IsStarted = false;
			ServerLogger.LogInformation("Server Stopped");
		}
		public static void HandleDisconnect(ClientHandler handler)
		{
			ServerLogger.LogInformation("Client", handler.id, "requested disconnect.");
			if (ActiveConnections.Remove(handler))
				ServerLogger.LogInformation("Successfully removed client", handler.id, "from registry.");
			else
				ServerLogger.LogError("Failed to remove client", handler.id, "from registry.");
		}
	}
}
