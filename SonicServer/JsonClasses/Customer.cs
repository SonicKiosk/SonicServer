using Newtonsoft.Json;

namespace SonicServer.JsonClasses
{
	public struct Customer
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public CustomerInfo CustomerInfo { get; set; }
	}
}
