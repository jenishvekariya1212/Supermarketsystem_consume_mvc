using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.BAL;

namespace Supermarketsystem.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    [Route("Admin/{Controller}/{Action}")]
    public class DashboardController : Controller
    {
        Uri baseuri = new Uri("http://localhost:5071/api");
        private readonly HttpClient _Client;
        public DashboardController()
        {
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
        }
        public IActionResult GET()
        {
            List<DashboardModel> dashboards = new List<DashboardModel>();
            HttpResponseMessage response = _Client.GetAsync($"{_Client.BaseAddress}/Dashboard/Get").Result;
            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);
                var dataofobject = jsonobject.data;
                var extractedDtaJson = JsonConvert.SerializeObject(dataofobject, Formatting.Indented);
                dashboards = JsonConvert.DeserializeObject<List<DashboardModel>>(extractedDtaJson);
            }
      

            return View("DashboardList",dashboards);
        }
    }
}
