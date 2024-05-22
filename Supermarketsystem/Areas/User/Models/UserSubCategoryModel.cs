namespace Supermarketsystem.Areas.User.Models
{
	public class UserSubCategoryModel
	{
		public int SubCategoryID { get; set; }

		public string SubCategoryName { get; set; }

		public string SubCategoryImage { get; set; }


		public int? CategoryID { get; set; }

		public string? CategoryName { get; set; }
	}
}
