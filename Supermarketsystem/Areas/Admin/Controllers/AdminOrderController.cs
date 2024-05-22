using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.Areas.User.Models;
using Supermarketsystem.BAL;

namespace Supermarketsystem.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    [Route("Admin/{Controller}/{Action}")]
    public class AdminOrderController : Controller
    {

        Uri baseuri = new Uri("http://localhost:5071/api");
        private readonly HttpClient _Client;
        public AdminOrderController()
        {
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
        }
        public IActionResult GET()
        {
            List<OrderModel> categories = new List<OrderModel>();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Order/Adminorder").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                categories = JsonConvert.DeserializeObject<List<OrderModel>>(extractedDtaJson);
            }
            return View("AdminOrderSee", categories);
        }
    }
}
