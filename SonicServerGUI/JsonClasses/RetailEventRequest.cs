using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SonicServer.JsonClasses
{
	public class RetailEventRequest
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public ResponseType Type { get; set; }
		public string For { get; set; }
		public string Verb { get; set; }
		public string Resource { get; set; }
		[JsonProperty("Payload Retail")]
		public PayloadRetail PayloadRetail { get; set; }
	}
}
