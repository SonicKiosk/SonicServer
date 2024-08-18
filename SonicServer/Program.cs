using SonicServer.JsonClasses;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SonicServer
{
	class Program
	{
		private static ClientHandler _clientHandler = null!;

		static void Main(string[] args)
		{
			Timer timer = new()
			{
				Interval = 20000,
				Enabled = true
			};
			timer.Elapsed += TimerOnElapsed;
			timer.Interval = 20000;
			timer.Enabled = true;
			StartServer();
		}

		private static void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			RetailEventRequest retailEventRequest;
			retailEventRequest = new RetailEventRequest
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
						Total = "420.69",
						Tax = "13.37",
						EmployeeFirstName = "Silly",
						EmployeeLastName = "Billy",
						SubTicketList = new List<SubTicket>{
							new() {
								EntryList = new List<Entry>{
									new() {
										ItemId = "COCK",
										MktgDescription = "COCK",
										Category = "COCK",
										Price = "69",
										Quantity = 69
									}
								}
							}
						}
					}
				}
			};
			ClientHandler.RetailEvent(
				_clientHandler.Stream,
				retailEventRequest.Verb,
				retailEventRequest.Resource,
				retailEventRequest.PayloadRetail
			);
			retailEventRequest = new RetailEventRequest
			{
				Verb = "CHECKIN",
				Resource = "/customer",
				PayloadRetail = new PayloadRetail()
				{
					Customer = new Customer
					{
						CustomerInfo = new CustomerInfo()
						{
							ID = "meow",
							FirstName = "FUcking",
							LastName = "Cocks",
							ProfilePictureUrl = "https://jack.polancz.uk/th.jpg"
						}
					}
				}
			};
			ClientHandler.RetailEvent(_clientHandler.Stream, retailEventRequest.Verb, retailEventRequest.Resource, retailEventRequest.PayloadRetail);
		}
		static void StartServer()
		{
			// Automatic host IP address retrival
			IPAddress address = Array.Find(
				Dns.GetHostEntry(Dns.GetHostName()).AddressList,
				(x) => x.AddressFamily == AddressFamily.InterNetwork
			);

			string host = "192.168.1.109"; // localhost
			if (address != null)
			{
				host = address.ToString();
			}
			int port = 8005; // choose any available port

			// Create a TCP listener
			TcpListener server = new(IPAddress.Parse(host), port);

			// Start listening for incoming connections
			server.Start();
			Console.WriteLine($"Server listening on {host}:{port}");
			while (true)
			{
				// Accept a connection from a client
				TcpClient client = server.AcceptTcpClient();
				Console.WriteLine($"Connection from {((IPEndPoint)client.Client.RemoteEndPoint!).Address}");
				// Handle the client request using ClientHandler
				_clientHandler = new ClientHandler(client);
				_clientHandler.HandleClient();
			}
		}
	}
}