namespace Supermarketsystem.Areas.Admin.Models
{
    public class CategoryModel
    {

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }
    }

    /*category dropdown*/

    public class Categorydropdown
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
