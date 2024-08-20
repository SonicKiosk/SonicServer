using Newtonsoft.Json;
using SonicServer.JsonClasses;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using SimpleLogs4Net;
using System;

namespace SonicServer
{
	public class ClientHandler
	{
		private const int ClientBufferSize = 1024;
		private readonly NetworkStream _stream;
		public HSStatus HandShakeStatus = HSStatus.NotStarted;
		private Dictionary<string, object[]> _existingItems;

		public ClientHandler(TcpClient client)
		{
			_stream = client.GetStream();
			HandleClient();
		}
		public NetworkStream Stream => _stream;

		public void HandleClient()
		{
			byte[] buffer = new byte[ClientBufferSize];
			int bytesRead;
			StringBuilder messageBuilder = new StringBuilder(); // TODO: Use string? should be faster
			int contentLength = -1;

			while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) != 0)
			{
				string messagePart = Encoding.UTF8.GetString(buffer, 0, bytesRead);
				if (HandShakeStatus != HSStatus.Success)
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
						Log.Write("RECEIVED: " + completeMessage);
						JsonData info = JsonConvert.DeserializeObject<JsonData>(completeMessage);
						JsonData info2 =
							JsonConvert.DeserializeObject<JsonData>(
								Encoding.UTF8.GetString(
									Convert.FromBase64String(info.Payload)
								)
							);
						Log.Write(Encoding.UTF8.GetString(Convert.FromBase64String(info2.Payload)));
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
		private void HandShake(string msg, NetworkStream stream)
		{
			HandShakeStatus = HSStatus.InProgress;
			JsonData response;
			if (msg.Contains("CMD hi")) {
				Log.Write("we have handshake p1 sending hi back", EType.Informtion);
				Send(stream, "RESP", "hi", null, null);
			} else if (msg.Contains("CMD capabilities")){
				Log.Write("we have handshake p2 sending capabilites", EType.Informtion);
				SendJson(stream, "RESP", "capabilities", null);
			} else if (msg.Contains("\"For\":\"devicetype\"")) {
				Log.Write("sending device info", EType.Informtion);
				response = new JsonData
				{
					For = "devicetype",
					Payload = "",
					Type = "RESP"
				};
				SendJson(stream, "DATA", null, response);
			} else if (msg.Contains("\"For\":\"stalllogin\"")){
				Log.Write("sending stall info", EType.Informtion);
				response = new JsonData
				{
					For = "stalllogin",
					Payload = "",
					Type = "RESP"
				};
				SendJson(stream, "DATA", null, response);
				Log.Write("Handshake complete! ", EType.Informtion);
				HandShakeStatus = HSStatus.Success;
			}
		}
		public void Send(NetworkStream conn, string method, string path, Dictionary<string, string> headers, string body)
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
			Log.Write(payload);
			conn.Write(buffer, 0, buffer.Length);
		}

		public  void SendJson(NetworkStream conn, string method, string path, object obj)
		{
			Dictionary<string, string> headers = new Dictionary<string, string>() {
				{ "Content-Type", "text/json" }
			};
			string body = obj != null ? JsonConvert.SerializeObject(obj) : "{}";
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
			JsonData payload = new JsonData()
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
				object[] objects = { desc, category, price, quantity, imagePath };
				_existingItems.Add(itemId, objects);
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
							new SubTicket() {
								EntryList = new List<Entry>{
									new Entry() {
										ItemId = "1002",
										MktgDescription = "COCK",
										Category = "COCK",
										Price = "69",
										Quantity = 69,
										ImagePath = "../../../../../../../test.png",
										ModifierList = new List<ModifierList>{ 
											new ModifierList()
											{
												ModifierId = "morecock",
												MktgDescription = "more cock"
											}
										}
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