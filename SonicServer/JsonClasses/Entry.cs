namespace SonicServer.JsonClasses
{
	public struct Entry
	{
		public string ItemId { get; set; }
		public string MktgDescription { get; set; }
		public string Category { get; set; }
		public string Price { get; set; }
		public int Quantity { get; set; }
		//public string ImagePath { get; set; }
		public List<Modifier> ModifierList { get; set; }
	}
}
