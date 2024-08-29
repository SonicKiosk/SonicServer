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
    public class SServer
    {
        private static Logger ServerLogger = new Logger("Server", Color.CornflowerBlue);

        public static List<ClientHandler> _activeConnections = new List<ClientHandler>();
        public static Settings? settings { get; private set; }
        public static SUI? ServerUI { get; private set; }
        public static Thread? UIThread { get; private set; }


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

        static void UIThreadBody(TcpListener server, string host, int port)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServerUI = new SUI(server, host, port);
            Application.Run(ServerUI);
            ServerUI.UpdateClients();
        }
        const string joeHeader = @"
*                           Sonic C# Server.                            *
*                     i dont recommend resizing lol                     *

*        _  ____  ______   ____  _____            _   _  _____ _    _   *
*       | |/ __ \|  ____| |  _ \|  __ \     /\   | \ | |/ ____| |  | |  *
*       | | |  | | |__    | |_) | |__) |   /  \  |  \| | |    | |__| |  *
*   _   | | |  | |  __|   |  _ <|  _  /   / /\ \ | . ` | |    |  __  |  *
*  | |__| | |__| | |____  | |_) | | \ \  / ____ \| |\  | |____| |  | |  *
*   \____/ \____/|______| |____/|_|  \_\/_/    \_\_| \_|\_____|_|  |_|  *
";

        static void StartServer()
        {
            if (Console.WindowWidth < joeHeader.Split('\n').Max(line => line.Length))
                Logger.LogHeader("_-", "Sonic C# Server.", Color.DodgerBlue, Color.DeepSkyBlue, 2);
            else
                Logger.LogHeader("_-", joeHeader, Color.DodgerBlue, Color.DeepSkyBlue, 2);

            ServerLogger.Newline(2);
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


            if (settings.DebugIP != null)
            {
                ServerLogger.Debug($"IP override found. Using ip {settings.DebugIP} (\"DebugIP\")");

            }
            //if (settings.DebugIP == null) // yucky cluttered block
            //{
            //    ServerLogger.Info("Host IP was null. Enter IP address:");
            //    settings.DebugIP = Console.ReadLine()?.Trim();
            //    ServerLogger.Info("Saving choice.");
            //    settings.SaveToFile();
            //    ServerLogger.Newline();
            //}

            string host = settings.DebugIP ?? "0.0.0.0";
            int port = 8005; // choose any available port
                             // Create a TCP listener
            ServerLogger.Info($"Attempting to create server on {host}:{port}.");
            TcpListener? server = null;
            try
            {
                server = new(!string.IsNullOrWhiteSpace(settings.DebugIP) ? IPAddress.Parse(settings.DebugIP) : IPAddress.Any, port);
                server.Start();
                Thread.Sleep(2000);
                //ServerLogger.Debug("Actual host:", server.LocalEndpoint.ToString());

                ServerLogger.Info("Spawning UI Thread.");
                UIThread = new Thread(() => UIThreadBody(server, host, port));
                UIThread.SetApartmentState(ApartmentState.STA);
                UIThread.Start();
            }
            catch (Exception ex)
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
            ServerLogger.Info($"Server listening on {server.LocalEndpoint.ToString()}");
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
                        ServerUI?.Invoke(ServerUI.UpdateClients);
                        //ServerUI?.UpdateClients();
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