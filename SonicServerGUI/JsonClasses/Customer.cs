using Newtonsoft.Json;

namespace SonicServer.JsonClasses
{
	public class Customer
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public CustomerInfo CustomerInfo { get; set; }
	}
}
