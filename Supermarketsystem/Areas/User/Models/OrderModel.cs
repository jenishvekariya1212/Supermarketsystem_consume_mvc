namespace Supermarketsystem.Areas.User.Models
{
	public class OrderModel
	{
		public int OrderID { get; set; }
		public int CartID { get; set; }


		public string? ProductName { get; set; }
		public string? ProductImage { get; set; }

		public decimal? ProductPrice { get; set; }
		public int? Quantity { get; set; }

		public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime OrderDate { get; set; }
	}
}
