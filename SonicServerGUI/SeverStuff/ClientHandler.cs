using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SonicServer.JsonClasses;
using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SonicServer.GUI;
using System.Threading;

namespace SonicServer
{
	public class ClientHandler
	{
		ILogger ClientLogger = null;
		private const int ClientBufferSize = 2048; // doubled the size since packets can be large asl
		private NetworkStream _stream;
		private readonly TcpClient client;
		public HSStatus HandShakeStatus = HSStatus.NotStarted;
		public Guid id { get; private set; } = Guid.Empty;
		private Dictionary<string, object[]> _existingItems = new Dictionary<string, object[]>();

		public ClientHandler(TcpClient client)
		{
			this.client = client;
			this.id = Guid.NewGuid();
			var loggerFactory = new LoggerFactory();
			ClientLogger = new LoggerFactory().CreateLogger($"Client {id}");
			ClientLogger.LogDebug("working");

			new Thread(() =>
			{
				_stream = client.GetStream();
				ClientLogger.LogInformation("Initialized ClientHandler.");
				HandleClient();
			}).Start();

			new Thread(CheckForDisconnect).Start();
		}
		public NetworkStream Stream => _stream;
		public void Disconnect()
		{
			ClientLogger.LogInformation("Disconnecting.");
			Server.HandleDisconnect(this);
		}
		public void CheckForDisconnect()
		{
			while (client.Connected && client.Client.Connected)
				continue;

			Disconnect();
		}
		public void HandleClient()
		{
			byte[] buffer = new byte[ClientBufferSize];
			int bytesRead;
			StringBuilder messageBuilder = new();
			int contentLength = -1;
			if (_stream == null || !_stream.CanRead)// || !_stream.Socket.Connected)   <- Doesn't exist in .NET Framework
			{
				Disconnect();
				return;
			}

			while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) != 0)
			{
				string messagePart = Encoding.UTF8.GetString(buffer, 0, bytesRead);
				if (HandShakeStatus != HSStatus.Success || HandShakeStatus != HSStatus.Failure)
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
						JObject decoded =
								JsonConvert.DeserializeObject<JObject>(
									Encoding.UTF8.GetString(
										Convert.FromBase64String(
											JsonConvert.DeserializeObject<JsonData>(
												Encoding.UTF8.GetString(
													Convert.FromBase64String(info.Payload) // fucking hell this is so stupppid
												)
											)!.Payload
										)
									)
								)!;
						//Console.ForegroundColor = ConsoleColor.Green;
						if (HandShakeStatus == HSStatus.Success)
						{
							ClientLogger.LogDebug("Payload:", decoded.ToString());
						}
						// Encoding.UTF8.GetString(Convert.FromBase64String(decoded.Payload)).Pastel(Color.DarkSalmon));
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
					if (int.TryParse(line.Substring("Content-Length:".Length).Trim(), out int contentLength))
					{
						return contentLength;
					}
				}
			}
			return -1; // Return -1 if Content-Length header is not found
		}
		private void HandShake(string message, NetworkStream stream)
		{
			HandShakeStatus = HSStatus.InProgress;
			JsonData response;
			switch (message)
			{
				case { } msg when msg.Contains("CMD hi"): // init, hi
					ClientLogger.LogInformation("Sending HI");
					Send(stream, "RESP", "hi", null, null);
					break;
				case { } msg when msg.Contains("CMD capabilities"): // init 2, capabilities
					ClientLogger.LogInformation("Sending Capabilities");
					SendJson(stream, "RESP", "capabilities", null);
					break;
				case { } msg when msg.Contains("\"For\":\"devicetype\""): // init 3, send device info
					ClientLogger.LogInformation("Sending device info");
					response = new JsonData
					{
						For = "devicetype",
						Payload = "",
						Type = ResponseType.RESP
					};
					SendJson(stream, "DATA", null, response);
					break;
				case { } msg when msg.Contains("\"For\":\"stalllogin\""): // init 3, send device (stall) info
					ClientLogger.LogInformation("Sending stall info");
					response = new JsonData
					{
						For = "stalllogin",
						Payload = "",
						Type = ResponseType.RESP
					};
					SendJson(stream, "DATA", null, response);
					RetailEventUtils.Checkin(this, new Customer()
					{
						CustomerInfo = new CustomerInfo()
						{
							ID = "joee",
							FirstName = "Joe",
							LastName = "Swanson",
							//Message = "Pluh",
							ProfilePictureUrl = "https://vignette.wikia.nocookie.net/universe-of-smash-bros-lawl/images/7/77/Joe.png/revision/latest?cb=20190422095447"
						}
					});
					RetailEventUtils.Ticket(this, RetailEventUtils.GetDummyTicket());
					AddItem("1", "2", "3", "4", 0, "5");
					ClientLogger.LogInformation("Handshake complete!");
					HandShakeStatus = HSStatus.Success;
					break;
			}
		}
		public void Send(NetworkStream conn, string? method, string? path, Dictionary<string, string>? headers, string? body)
		{
			StringBuilder payload = new StringBuilder($"{method?.Trim() ?? ""} {path?.Trim() ?? ""}\r\n");

			if (headers != null)
				foreach (KeyValuePair<string, string> header in headers)
					payload.Append($"{header.Key}: {header.Value}\r\n");

			payload.Append("\r\n");
			if (!string.IsNullOrWhiteSpace(body))
				payload.Append(body.Trim().Replace('\r', '\0').Replace("\n", "\r\n"));

			payload.Append("\r\n");


			//string payload = $"{method?.Trim() ?? ""} {path?.Trim() ?? ""}\r\n";
			//if (headers != null)
			//{
			//    foreach (var header in headers)
			//    {
			//        payload += header.Key + ": " + header.Value + "\r\n"; // I know that it looks worse than stringbuilder but it's faster
			//    }
			//}
			//payload += "\r\n";
			//if (!string.IsNullOrEmpty(body))
			//{
			//    payload += body.Trim().Replace('\r', '\0').Replace("\n", "\r\n");
			//}
			//payload += "\r\n";

			byte[] buffer = Encoding.UTF8.GetBytes(payload.ToString());
			//ClientLogger.Debug(payload);
			conn.Write(buffer, 0, buffer.Length);
		}

		public void SendJson(NetworkStream conn, string method, string? path, object? obj)
		{
			Dictionary<string, string> headers = new() {
				{ "Content-Type", "text/json" }
			};
			string? body = obj != null ? JsonConvert.SerializeObject(obj) : "{}";
			ClientLogger.LogDebug(body);
			headers["Content-Length"] = Encoding.UTF8.GetByteCount(body).ToString();
			Send(conn, method, path, headers, body);
		}
		public string B64Json(object data)
		{
			string jsonString = JsonConvert.SerializeObject(data);
			byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
			return Convert.ToBase64String(bytes);
		}
		public void RetailEvent(NetworkStream conn, string verb, string resource, dynamic body)
		{
			JsonData payload = new()
			{
				Type = ResponseType.DATA,
				For = "stall",
				Payload = B64Json(new JsonData
				{
					Type = ResponseType.DATA,
					For = "stall",
					ServiceId = "retail",
					Payload = B64Json(new RetailEventRequest
					{
						Type = ResponseType.RQST,
						For = "retail",
						Verb = verb,
						Resource = resource,
						PayloadRetail = body
					})
				})
			};
			SendJson(conn, "DATA", "", payload);
		}

		public void AddItem(string itemId, string desc, string category, string price, int quantity, string imagePath)
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
				Type = ResponseType.RQST,
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
										ModifierList = new List<Modifier>{ new()
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