using Pastel;
using SonicServer.JsonClasses;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SonicServer
{
	class Program
	{
		private static Logger ServerLogger = new Logger("Server", Color.CornflowerBlue);

		public static List<ClientHandler> _activeConnections = new List<ClientHandler>();
		public static Settings? settings { get; private set; }


        static void Main(string[] args)
		{
			// Timer timer = new()
			// {
			// 	Interval = 10000,
			// 	Enabled = true
			// };
			// timer.Elapsed += TimerOnElapsed;
			// timer.Interval = 20000;
			// timer.Enabled = true;
			StartServer();
		}

		// private static void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		// {
		// 	RetailEventRequest retailEventRequest;
		// 	retailEventRequest = new RetailEventRequest
		// 	{
		// 		Type = "RQST",
		// 		For = "retail",
		// 		Verb = "POST",
		// 		Resource = "/retail/ticket",
		// 		PayloadRetail = new PayloadRetail
		// 		{
		// 			Ticket = new Ticket
		// 			{
		// 				State = "ACTIVE",
		// 				Total = "420.69",
		// 				Tax = "13.37",
		// 				EmployeeFirstName = "Silly",
		// 				EmployeeLastName = "Billy",
		// 				SubTicketList = new List<SubTicket>{
		// 					new() {
		// 						EntryList = new List<Entry>{
		// 							new() {
		// 								ItemId = "1002",
		// 								MktgDescription = "COCK",
		// 								Category = "COCK",
		// 								Price = "69",
		// 								Quantity = 69,
		// 								ImagePath = "../../../../../../../test.png",
		// 								ModifierList = new List<ModifierList>{ new()
		// 								{
		// 									ModifierId = "morecock",
		// 									MktgDescription = "more cock"
		// 								}}
		// 							}
		// 						}
		// 					}
		// 				}
		// 			}
		// 		}
		// 	};
		// 	// for (int i = 0; i < 100; i++)
		// 	// {
		// 	// 	retailEventRequest.PayloadRetail.Ticket.SubTicketList[0].EntryList[0].ModifierList.Add(new()
		// 	// 	{
		// 	// 		ModifierId = "morecock",
		// 	// 		MktgDescription = "more cock"
		// 	// 	});
		// 	// }
		// 	ClientHandler.RetailEvent(
		// 		_clientHandler.Stream,
		// 		retailEventRequest.Verb,
		// 		retailEventRequest.Resource,
		// 		retailEventRequest.PayloadRetail
		// 	);
		// 	retailEventRequest = new RetailEventRequest
		// 	{
		// 		Verb = "CHECKIN",
		// 		Resource = "/customer",
		// 		PayloadRetail = new PayloadRetail()
		// 		{
		// 			Customer = new Customer
		// 			{
		// 				CustomerInfo = new CustomerInfo()
		// 				{
		// 					ID = "meow",
		// 					FirstName = "FUcking",
		// 					LastName = "Cocks",
		// 					ProfilePictureUrl = "https://jack.polancz.uk/th.jpg"
		// 				}
		// 			}
		// 		}
		// 	};
		// 	ClientHandler.RetailEvent(_clientHandler.Stream, retailEventRequest.Verb, retailEventRequest.Resource, retailEventRequest.PayloadRetail);
		// }
		public static void HandleDisconnect(ClientHandler handler)
		{
			ServerLogger.Info("Client", handler.id.ToString().Pastel(Color.IndianRed), "requested disconnect.");
			if (_activeConnections.Remove(handler))
				ServerLogger.Info("Successfully removed client", handler.id.ToString().Pastel(Color.IndianRed), "from registry.");
            else
                ServerLogger.Error("Failed to remove client", handler.id.ToString().Pastel(Color.IndianRed), "from registry.");
        }
		static void StartServer()
		{
			Logger.LogHeader("=-", "Sonic C# Server.", Color.Yellow, Color.Gold, 1);
            //log test
            //Logger test = new Logger("test", Color.CornflowerBlue);// + Logger.ConsoleStyles.BOLD);
            //ServerLogger.Info("hi", 231, "param3");
            //ServerLogger.Error("hello", null, 2.4f);
            //ServerLogger.Warn("hey", true, '\n');

            // Automatic host IP address retrival - this shit NEVER works. using readline with a config.
            //IPAddress address = Array.Find(
            //	Dns.GetHostEntry(Dns.GetHostName()).AddressList,
            //	(x) => x.AddressFamily == AddressFamily.InterNetwork
            //);

            //string host = address.ToString(); //"192.168.1.109"; // localhost
            //! address = "192.168.1.109"
            ServerLogger.Info("Reading config from cfg.json");
            ServerLogger.Newline();
            settings = Config.ReadFromFile();
			
			//if (settings.DebugIP != null)
			//{
			//	ServerLogger.Debug($"IP override found. Using ip {settings.DebugIP} (\"DebugIP\")");

			//}
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