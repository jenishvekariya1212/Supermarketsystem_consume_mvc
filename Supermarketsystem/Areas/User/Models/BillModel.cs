namespace Supermarketsystem.Areas.User.Models
{
    public class BillModel
    {
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }


        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        public int UserID { get; set; }
    }
}
