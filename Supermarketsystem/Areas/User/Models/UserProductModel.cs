﻿namespace Supermarketsystem.Areas.User.Models
{
	public class UserProductModel
	{

		public int ProductID { get; set; }
		

		public string ProductName { get; set; }

		public int? CategoryID { get; set; }

		public string? CategoryName { get; set; }

		public int? SubCategoryID { get; set; }

		public string? SubCategoryName { get; set; }

		public int ProductQuantity { get; set; }

		public decimal ProductPrice { get; set; }

		public DateTime ProductExpiryDate { get; set; }

		public string ProductImage { get; set; }

		public DateTime? Created { get; set; }

		public DateTime? Modified { get; set; }
	}
}
