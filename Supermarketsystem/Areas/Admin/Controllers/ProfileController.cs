using Microsoft.AspNetCore.Mvc;
using Supermarketsystem.Areas.Login.Models;
using Supermarketsystem.BAL;
using System.Reflection;

namespace Supermarketsystem.Areas.Admin.Controllers
{
    [CheckAccess]
    [Area("Admin")]
    [Route("Admin/{Controller}/{Action}")]
    public class ProfileController : Controller
    {
        public IActionResult GET()
        {
            //string firstName = HttpContext.Session.GetString("FirstName");
            string firstName = HttpContext.Session.GetString("FirstName");
            string lastName = HttpContext.Session.GetString("LastName");
            string email = HttpContext.Session.GetString("Email");
            string username = HttpContext.Session.GetString("UserName");
            ViewData["firstName"] = firstName;
            ViewData["lastName"] = lastName;
            ViewData["email"] = email;
            ViewData["username"] = username;
            return View("ProfileList");
        }
    }
}
