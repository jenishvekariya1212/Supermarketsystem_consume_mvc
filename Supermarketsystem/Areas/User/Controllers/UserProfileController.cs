using Microsoft.AspNetCore.Mvc;
using Supermarketsystem.BAL;

namespace Supermarketsystem.Areas.User.Controllers
{
    [CheckAccess]
		[Area("User")]
		[Route("User/{Controller}/{Action}")]
    public class UserProfileController : Controller
    {
		
		public IActionResult GET()
        {
            string firstName = HttpContext.Session.GetString("FirstName");
            string lastName = HttpContext.Session.GetString("LastName");
            string email = HttpContext.Session.GetString("Email");
            string username = HttpContext.Session.GetString("UserName");
            ViewData["firstName"] = firstName;
            ViewData["lastName"] = lastName;
            ViewData["email"] = email;
            ViewData["username"] = username;

            return View("UserProfileList");
        }
    }
}
