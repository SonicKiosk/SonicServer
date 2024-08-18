using Newtonsoft.Json;
using System.Collections.Generic;

public class SubTicket
{
    public List<Entry> EntryList { get; set; }
}

public class Entry
{
    public string ItemId { get; set; }
    public string MktgDescription { get; set; }
    public string Category { get; set; }
    public string Price { get; set; }
    public int Quantity { get; set; }
}

public class Ticket
{
    public string State { get; set; }
    public string Total { get; set; }
    public string Tax { get; set; }
    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public List<SubTicket> SubTicketList { get; set; }
}

public class CustomerInfo
{
    public string ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureUrl { get; set; }
}

public class Customer
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CustomerInfo CustomerInfo { get; set; }
}
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class PayloadRetail
{
    public Ticket Ticket { get; set; }
    public Customer? Customer { get; set; }
}

public class RetailEventRequest
{
    [JsonProperty("Type")]
    public string Type { get; set; }
    
    [JsonProperty("For")]
    public string For { get; set; }
    
  
    [JsonProperty("Verb")]
    public string Verb { get; set; }
    
    [JsonProperty("Resource")]
    public string Resource { get; set; }

    [JsonProperty("Payload Retail")]
    public PayloadRetail PayloadRetail { get; set; }
}
 public class JsonData
    {
        [JsonProperty("For")] public string For { get; set; }
        [JsonProperty("Payload")] public string Payload { get; set; }
        [JsonProperty("ServiceId")] public string ServiceId { get; set; }
        [JsonProperty("Type")] public string Type { get; set; }
    }