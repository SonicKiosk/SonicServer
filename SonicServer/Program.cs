using Pastel;
using ServerUI;
using SonicServer.JsonClasses;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Web;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace SonicServer
{
	class Program
	{
		private static Logger ServerLogger = new Logger("Server", Color.CornflowerBlue);

		public static List<ClientHandler> _activeConnections = new List<ClientHandler>();
		public static Settings? settings { get; private set; }
		public static SUI? ServerUI { get; private set; }


        static void Main(string[] args)
		{
			
			StartServer();
		}

		
		public static void HandleDisconnect(ClientHandler handler)
		{
			ServerLogger.Info("Client", handler.id.ToString().Pastel(Color.IndianRed), "requested disconnect.");
			if (_activeConnections.Remove(handler))
				ServerLogger.Info("Successfully removed client", handler.id.ToString().Pastel(Color.IndianRed), "from registry.");
            else
                ServerLogger.Error("Failed to remove client", handler.id.ToString().Pastel(Color.IndianRed), "from registry.");
        }

		static void UIThread(TcpListener server, string host, int port)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			ServerUI = new SUI(server, host, port);
            Application.Run(ServerUI);
        }
		static void StartServer()
		{
			Logger.LogHeader("=-", "Sonic C# Server.", Color.Yellow, Color.Gold, 1);
            ServerLogger.Info("Reading config from cfg.json");
            ServerLogger.Newline();
            settings = Config.ReadFromFile();
            if (settings.DebugIP == null) // yucky cluttered block
			{
				ServerLogger.Info("Host IP was null. Enter IP address:");
				settings.DebugIP = Console.ReadLine()?.Trim();
				ServerLogger.Info("Saving choice.");
				settings.SaveToFile();
				ServerLogger.Newline();
			}

			string host = settings.DebugIP ?? "127.0.0.1";
			int port = 8005; // choose any available port
                             // Create a TCP listener
            ServerLogger.Info($"Attempting to create server on {host}:{port}.");
			TcpListener? server = null;
            try
            {
                server = new(IPAddress.Parse(host), port);
                server.Start();

				new Thread(() => UIThread(server, host, port)).Start();
            }
			catch(Exception ex)
			{
				ServerLogger.Error($"Error starting server. Fatal. Message: {ex.Message}");
				ServerLogger.Error("Quitting.");
				Environment.Exit(1);
			}

			if (server == null)
            {
                ServerLogger.Error($"Server was null. Quitting.");
                Environment.Exit(1);
            }
            // Start listening for incoming connections
            ServerLogger.Info($"Server listening on {host}:{port}");
            ServerLogger.Newline();
            while (true)
			{
				// TODO: make not throw
				// Accept a connection from a client
				try
                {
                    TcpClient? client = server.AcceptTcpClient();
					if (client != null && client.Connected)
                    {
                        ServerLogger.Info($"Connection from {((IPEndPoint)client.Client.RemoteEndPoint!).Address}");
						// Handle the client request using ClientHandler
                        _activeConnections.Add(new ClientHandler(client));
                    }
                }
                catch (Exception ex)
                {
                    ServerLogger.Error($"Error accepting client. Fatal. Message: {ex.Message}");
                    ServerLogger.Error("Quitting.");
                    Environment.Exit(1);
                }
			}
		}
	}
}
