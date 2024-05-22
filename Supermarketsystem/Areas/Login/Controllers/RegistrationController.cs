using Microsoft.AspNetCore.Mvc;
using Supermarketsystem.Areas.Admin.Models;
using Supermarketsystem.Areas.Login.DAL;
using Supermarketsystem.Areas.Login.Models;
using System.Reflection;

namespace Supermarketsystem.Areas.Login.Controllers
{
    [Area("Login")]
    [Route("Login/{Controller}/{Action}")]
    
    public class RegistrationController : Controller
    {
        Uri baseuri = new Uri("http://localhost:5071/api");
        DAL_Helper dal = new DAL_Helper();
        private readonly HttpClient _Client;
        public RegistrationController()
        {
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(Registraion registraion)
        {
            try
            {
                MultipartFormDataContent fromdata = new MultipartFormDataContent();
                fromdata.Add(new StringContent(registraion.FirstName), "FirstName");
                fromdata.Add(new StringContent(registraion.LastName), "LastName");
                fromdata.Add(new StringContent(registraion.UserName), "UserName");
                fromdata.Add(new StringContent(registraion.Password), "Password");
                fromdata.Add(new StringContent(registraion.Email), "Email");
                fromdata.Add(new StringContent(Convert.ToString(registraion.ISActive)), "ISActive");


                HttpResponseMessage response = await _Client.PostAsync($"{_Client.BaseAddress}/Login/Post", fromdata);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Person Inserted";
                    return RedirectToAction("Login");
                }
            }
            

            catch (Exception ex)
            {
                TempData["Error"] = "An Error Occured" + ex.Message;
            }
            return RedirectToAction("Login");
        }

        
        public IActionResult Login()
        {

            return View();
        }

        
       
        public async Task<IActionResult> LoginCheck(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("UserName", "Enter Username");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Enter Password");
            }
            string apiUrl = $"/Login/GetById/{model.UserName}/{model.Password}"; 
            Console.WriteLine(apiUrl);
            if (ModelState.IsValid)
            {
                var apiResponse = await dal.SendRequestAsync<Registraion>(apiUrl, HttpMethod.Get);
                Console.WriteLine("Login => "+ apiResponse.IsSuccess);


                if (apiResponse.IsSuccess)
                {
                   Registraion user = apiResponse.Data;
                    Console.WriteLine(user.ISActive);
                    HttpContext.Session.SetInt32("UserID", user.UserID);
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("LastName", user.LastName);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Password", user.Password);
                    HttpContext.Session.SetString("Email", user.Email);

                    HttpContext.Session.SetInt32("ISActive", Convert.ToInt32(user.ISActive));

                    if (HttpContext.Session.GetInt32("UserID") != null && HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetInt32("ISActive") != null)
                    {
                        if (HttpContext.Session.GetInt32("ISActive") == 1)
                        {
                            return RedirectToAction("GET", "Dashboard", new { area = "Admin" });
                        }
                        else
                        {
                            return RedirectToAction("GET", "UserCategory", new { area = "User" });
                        }
                    }
                    else
                    {
                        @ViewData["ErrorMessage"] = "Invalid Username or Password";

                    }

                   /* return RedirectToAction("GET", "Dashboard", new {area ="Admin"});*/

                }
                else
                {
                    @ViewData["ErrorMessage"] = "Invalid Username or Password";
                }
            }

            return View("Login",model);
        }



       
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
