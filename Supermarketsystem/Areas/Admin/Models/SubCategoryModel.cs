namespace Supermarketsystem.Areas.Admin.Models
{
    public class SubCategoryModel
    {
        public int SubCategoryID { get; set; }

        public string SubCategoryName { get; set; }

        public string SubCategoryImage { get; set; }

        /* public string SubCategoryImage { get; set; } // Add a property to store the image path

         public IFormFile SubCategoryImageFile { get; set; } // Add a property to store the image file for upload*/

        public int? CategoryID { get; set; }

        public string? CategoryName { get; set; }
    }
    public class SubcategoryDropDown
    {
        public int SubCategoryID { get; set; }

        public string SubCategoryName { get; set; }
    }

}
