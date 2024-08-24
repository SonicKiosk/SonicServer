using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SonicServer.JsonClasses
{
	public class JsonData
	{
		public string For { get; set; }
		public string Payload { get; set; }
		public string ServiceId { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public ResponseType Type { get; set; }
	}
}
