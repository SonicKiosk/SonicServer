using Newtonsoft.Json;

namespace SonicServer.JsonClasses
{
	public struct RetailEventRequest
	{
		public string Type { get; set; }
		public string For { get; set; }
		public string Verb { get; set; }
		public string Resource { get; set; }
		[JsonProperty("Payload Retail")]
		public PayloadRetail PayloadRetail { get; set; }
	}
}
