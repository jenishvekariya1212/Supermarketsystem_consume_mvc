using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.Areas.User.Models;
using Supermarketsystem.BAL;

namespace Supermarketsystem.Areas.User.Controllers
{
	[CheckAccess]	
	[Area("User")]
		[Route("User/{Controller}/{Action}")]
	public class UserSubCategoryController : Controller
	{
		Uri baseuri = new Uri("http://localhost:5071/api");
		private readonly HttpClient _Client;
		public UserSubCategoryController()
		{
			_Client = new HttpClient();
			_Client.BaseAddress = baseuri;
		}
		public IActionResult GETBYID(int CategoryID)
		{
			List<UserSubCategoryModel> Dropdown = new List<UserSubCategoryModel>();
			HttpResponseMessage response1 = _Client.GetAsync($"{_Client.BaseAddress}/SubCategory/GetByCategoryID/{CategoryID}").Result;
			if (response1.IsSuccessStatusCode)
			{

				string data = response1.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var dataofobject = jsonobject.data;
				var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
				Dropdown = JsonConvert.DeserializeObject<List<UserSubCategoryModel>>(extractedDtaJson);
			}
			return View("UserSubCategoryList",Dropdown);
		}
	}
}
