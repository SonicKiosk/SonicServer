namespace SonicServer.JsonClasses
{
	public class Ticket
	{
		public string State { get; set; }
		public string Total { get; set; }
		public string Tax { get; set; }
		public string EmployeeFirstName { get; set; }
		public string EmployeeLastName { get; set; }
		public List<SubTicket> SubTicketList { get; set; }
	}
}
