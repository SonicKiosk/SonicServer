using Newtonsoft.Json;
using Pastel;
using SonicServer.JsonClasses;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SonicServer
{
	public class ClientHandler
	{
		Logger ClientLogger = null;
		private const int ClientBufferSize = 1024;
		private NetworkStream _stream;
		private readonly TcpClient client;
		private bool _isHandShakeDone = false;
		public Guid id { get; private set; } = Guid.Empty;
		private Dictionary<string, object[]> _existingItems;

		public ClientHandler(TcpClient client)
		{
			this.client = client;
			this.id = Guid.NewGuid();
            //{((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString().Pastel(Color.MediumAquamarine)}
            ClientLogger = new Logger($"Client {id.ToString().Pastel(Color.MediumAquamarine)}", Color.Chartreuse);

			//ClientLogger.Info("herro");
			new Thread(() =>
            {
                _stream = client.GetStream();
                ClientLogger.Info("Initialized ClientHandler.");
                HandleClient();
            }).Start();

            new Thread(CheckForDisconnect).Start();
        }
		public NetworkStream Stream => _stream;
		public void CheckForDisconnect()
		{
			while (client.Connected && client.Client.Connected)
				continue;
			ClientLogger.Info("Disconnecting.");
		}
		public void HandleClient()
        {
            byte[] buffer = new byte[ClientBufferSize];
			int bytesRead;
			StringBuilder messageBuilder = new();
			int contentLength = -1;

			while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) != 0)
			{
				string messagePart = Encoding.UTF8.GetString(buffer, 0, bytesRead);
				if (!_isHandShakeDone)
				{
					HandShake(messagePart, _stream);
				}
				else
				{
					// Handle other messages after handshake
					messageBuilder.Append(messagePart);
					if (contentLength == -1)
					{
						// Check if we have received the headers
						string headers = messageBuilder.ToString();
						int headerEndIndex = headers.IndexOf("\r\n\r\n");
						if (headerEndIndex != -1)
						{
							// Extract headers
							string headerPart = headers.Substring(0, headerEndIndex);
							// Extract content length
							contentLength = GetContentLength(headerPart);
							// Remove headers from the message builder
							messageBuilder.Remove(0, headerEndIndex + 4);
						}
					}

					// Process the received data
					if (contentLength != -1 && messageBuilder.Length >= contentLength)
					{
						string completeMessage = messageBuilder.ToString(0, contentLength);
						//Console.ForegroundColor = ConsoleColor.DarkRed;
						//ClientLogger.Info("Received message.");
						//ClientLogger.Debug($"Recieved Message: {completeMessage.Pastel(Color.DarkRed)}");
						JsonData info = JsonConvert.DeserializeObject<JsonData>(completeMessage)!;
						JsonData info2 =
							JsonConvert.DeserializeObject<JsonData>(
								Encoding.UTF8.GetString(
									Convert.FromBase64String(info.Payload)
								)
							)!;
						//Console.ForegroundColor = ConsoleColor.Green;
                        //ClientLogger.Debug("Payload:",Encoding.UTF8.GetString(Convert.FromBase64String(info2.Payload)).Pastel(Color.DarkSalmon));
						//Console.ResetColor();
						// Clear the message builder for the next message
						messageBuilder.Clear();
						contentLength = -1;
					}
				}
			}
		}

		private int GetContentLength(string headers)
		{
			foreach (string line in headers.Split(new[] { "\r\n" }, StringSplitOptions.None))
			{
				if (line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
				{
					if (int.TryParse(line["Content-Length:".Length..].Trim(), out int contentLength))
					{
						return contentLength;
					}
				}
			}
			return -1; // Return -1 if Content-Length header is not found
		}
		private void HandShake(string message, NetworkStream stream)
		{
			JsonData response;
			switch (message)
			{
				case { } msg when msg.Contains("CMD hi"):
                    ClientLogger.Info("Responding hi ");
					Send(stream, "RESP", "hi", null, null);
					break;
				case { } msg when msg.Contains("CMD capabilities"):
					Console.WriteLine("we have handshake p2 sending capabilites");
					SendJson(stream, "RESP", "capabilities", null);
					break;
				case { } msg when msg.Contains("\"For\":\"devicetype\""):
					Console.WriteLine("sending device info");
					response = new JsonData
					{
						For = "devicetype",
						Payload = "",
						Type = "RESP"
					};
					SendJson(stream, "DATA", null, response);
					break;
				case { } msg when msg.Contains("\"For\":\"stalllogin\""):
					Console.WriteLine("sending stall info");
					response = new JsonData
					{
						For = "stalllogin",
						Payload = "",
						Type = "RESP"
					};
					SendJson(stream, "DATA", null, response);
					Console.WriteLine("Handshake complete! ");
					_isHandShakeDone = true;
					break;
			}
		}
		public  void Send(NetworkStream conn, string? method, string? path, Dictionary<string, string>? headers, string? body)
		{
			string payload = $"{method?.Trim() ?? ""} {path?.Trim() ?? ""}\r\n";
			if (headers != null)
			{
				foreach (var header in headers)
				{
					payload += header.Key + ": " + header.Value + "\r\n"; // I know that it looks worse than stringbuilder but it's faster
				}
			}
			payload += "\r\n";
			if (!string.IsNullOrEmpty(body))
			{
				payload += body.Trim().Replace('\r', '\0').Replace("\n", "\r\n");
			}
			payload += "\r\n";
			byte[] buffer = Encoding.UTF8.GetBytes(payload);
			Console.WriteLine(payload);
			conn.Write(buffer, 0, buffer.Length);
		}

		public  void SendJson(NetworkStream conn, string method, string? path, object? obj)
		{
			Dictionary<string, string> headers = new() {
				{ "Content-Type", "text/json" }
			};
			string? body = obj != null ? JsonConvert.SerializeObject(obj) : "{}";
			headers["Content-Length"] = Encoding.UTF8.GetByteCount(body).ToString();
			Send(conn, method, path, headers, body);
		}
		public  string B64Json(object data)
		{
			string jsonString = JsonConvert.SerializeObject(data);
			byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
			return Convert.ToBase64String(bytes);
		}
		public  void RetailEvent(NetworkStream conn, string verb, string resource, dynamic body)
		{
			JsonData payload = new()
			{
				Type = "DATA",
				For = "stall",
				Payload = B64Json(new JsonData
				{
					Type = "DATA",
					For = "stall",
					ServiceId = "retail",
					Payload = B64Json(new RetailEventRequest
					{
						Type = "RQST",
						For = "retail",
						Verb = verb,
						Resource = resource,
						PayloadRetail = body
					})
				})
			};
			SendJson(conn, "DATA", "", payload);
		}

		public void AddItem(string itemId, string desc, string category, string price, int quantity,string imagePath)
		{
			int total;
			if (_existingItems.TryGetValue(itemId, out var item))
			{
				// c# this is dumb as shit please fix it rn thx xx
				item[4] = (int)item[4] + 1;


			}
			else
			{
					_existingItems.Add(itemId,
						[desc, category, price, quantity, imagePath,]);
			}
			
			
			
			var retailEventRequest = new RetailEventRequest
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
										ItemId = "1002",
										MktgDescription = "COCK",
										Category = "COCK",
										Price = "69",
										Quantity = 69,
										ImagePath = "../../../../../../../test.png",
										ModifierList = new List<ModifierList>{ new()
										{
											ModifierId = "morecock",
											MktgDescription = "more cock"
										}}
									}
								}
							}
						}
					}
				}
			};
		}
	}
}