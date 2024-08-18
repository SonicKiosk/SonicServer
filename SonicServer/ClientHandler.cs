using Newtonsoft.Json;
using SonicServer.JsonClasses;
using System.Net.Sockets;
using System.Text;

namespace SonicServer
{
	public class ClientHandler
	{
		private const int ClientBufferSize = 1024;
		private readonly NetworkStream _stream;
		private static bool _isHandShakeDone = false;

		public ClientHandler(TcpClient client)
		{
			_stream = client.GetStream();
		}
		public NetworkStream Stream => _stream;

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
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.WriteLine($"RECEIVED: {completeMessage}");
						JsonData info = JsonConvert.DeserializeObject<JsonData>(completeMessage)!;
						JsonData info2 =
							JsonConvert.DeserializeObject<JsonData>(
								Encoding.UTF8.GetString(
									Convert.FromBase64String(info.Payload)
								)
							)!;
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(info2.Payload)));
						Console.ResetColor();
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
		private static void HandShake(string message, NetworkStream stream)
		{
			JsonData response;
			switch (message)
			{
				case { } msg when msg.Contains("CMD hi"):
					Console.WriteLine("we have handshake p1 sending hi back");
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
		public static void Send(NetworkStream conn, string? method, string? path, Dictionary<string, string>? headers, string? body)
		{
			string payload = $"{method ?? ""} {path ?? ""}\r\n".Trim();
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

		public static void SendJson(NetworkStream conn, string method, string? path, object? obj)
		{
			Dictionary<string, string> headers = new() {
				{ "Content-Type", "text/json" }
			};
			string? body = obj != null ? JsonConvert.SerializeObject(obj) : "{}";
			headers["Content-Length"] = Encoding.UTF8.GetByteCount(body).ToString();
			Send(conn, method, path, headers, body);
		}
		public static string B64Json(object data)
		{
			string jsonString = JsonConvert.SerializeObject(data);
			byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
			return Convert.ToBase64String(bytes);
		}
		public static void RetailEvent(NetworkStream conn, string verb, string resource, dynamic body)
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
	}
}