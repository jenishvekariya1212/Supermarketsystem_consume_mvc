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
    public class UserCategoryController : Controller
    {

		Uri baseuri = new Uri("http://localhost:5071/api");
		private readonly HttpClient _Client;
        public UserCategoryController()
        {
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
        }

			public IActionResult GET()
        {

            return View("UserCategoryList");
        }

        public IActionResult UserGet()
        {
			List<UserCategoryModel> categories = new List<UserCategoryModel>();
			HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Category/Get").Result;
			if (response.IsSuccessStatusCode)
			{

				string data = response.Content.ReadAsStringAsync().Result;
				dynamic jsonobject = JsonConvert.DeserializeObject(data);
				var dataofobject = jsonobject.data;
				var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
				categories = JsonConvert.DeserializeObject<List<UserCategoryModel>>(extractedDtaJson);
			}
			
			return View("UserCategoryAllList",categories);
        }
    }
}
