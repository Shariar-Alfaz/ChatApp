using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
