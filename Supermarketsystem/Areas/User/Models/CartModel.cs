namespace Supermarketsystem.Areas.User.Models
{
    public class CartModel
    {
        public int CartID { get; set; }

        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public decimal? ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int UserID { get; set; }
    }
}
