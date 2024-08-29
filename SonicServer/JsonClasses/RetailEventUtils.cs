using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                SubTicketList = new List<SubTicket>()
                    {
                        new SubTicket()
                        {
                            EntryList=new List<Entry>()
                            {
                            }
                        }
                    }
            };
        }
        public static void Checkin(ClientHandler client, Customer info)
        {
            client.RetailEvent(client.Stream, "CHECKIN", "/customer", new PayloadRetail() { Customer = info });
        }

        public static void Checkout(ClientHandler client, Customer info)
        {
            client.RetailEvent(client.Stream, "CHECKOUT", "/customer", new PayloadRetail() { Customer = info });
        }
        public enum CrashMethod
        {
            InvalidCheckoutPacket = 0,
        }
        public static void CrashClient(ClientHandler client, CrashMethod method)
        {
            switch (method)
            {
                case CrashMethod.InvalidCheckoutPacket:
                    client.RetailEvent(client.Stream, "CHECKOUT", "/customer", new PayloadRetail() { });
                    break;
            }
        }
        public static void Ticket(ClientHandler client, Ticket ticket)
        {
            client.RetailEvent(client.Stream, "POST", "/retail/ticket", new PayloadRetail() { Ticket = ticket });
        }
    }
}
