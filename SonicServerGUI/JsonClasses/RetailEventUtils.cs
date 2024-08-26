namespace SonicServer.JsonClasses
{
	class RetailEventUtils
	{
		public enum RetailEndpoint
		{
			CheckIn = 0,
			Ticket = 1
		}
		public static Ticket GetDummyTicket()
		{
			return new Ticket()
			{
				EmployeeFirstName = "N",
				EmployeeLastName = "A",
				State = "ACTIVE",
				Total = "0.00",
				Tax = "0.00",
				SubTicketList =
					[
						new SubTicket()
						{
							EntryList=[]
						}
					]
			};
		}
		public static void Checkin(ClientHandler client, Customer info)
		{
			client.RetailEvent(client.Stream, "CHECKIN", "/customer", new PayloadRetail() { Customer = info });
		}
		public static void Ticket(ClientHandler client, Ticket ticket)
		{
			client.RetailEvent(client.Stream, "POST", "/retail/ticket", new PayloadRetail() { Ticket = ticket });
		}
	}
}
