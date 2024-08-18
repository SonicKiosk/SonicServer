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
            Timer timer = new Timer();
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
                        SubTicketList = new List<SubTicket>
                        {
                            new SubTicket
                            {
                                EntryList = new List<Entry>
                                {
                                    new Entry
                                    {
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
            ClientHandler.RetailEvent(_clientHandler.Stream, retailEventRequest.Verb, retailEventRequest.Resource,
                retailEventRequest.PayloadRetail);
            retailEventRequest = new RetailEventRequest
            {
                Verb = "CHECKIN",
                Resource="/customer",
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
                string host = "192.168.1.109"; // localhost
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

                    // Handle the client request using ClientHandler
                    _clientHandler = new ClientHandler(client);
                    _clientHandler.HandleClient();
                }
            }
        }
    }
