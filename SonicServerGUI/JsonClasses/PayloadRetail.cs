using Newtonsoft.Json;

namespace SonicServer.JsonClasses
{
	[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	public class PayloadRetail
	{
		public Ticket Ticket { get; set; }
		public Customer Customer { get; set; }
	}
}
