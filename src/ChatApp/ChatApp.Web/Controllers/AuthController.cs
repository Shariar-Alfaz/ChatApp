using Autofac;
using ChatApp.Web.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILifetimeScope _scope;

        public AuthController(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IActionResult Login()
        {
           
            return View();
        }

        public IActionResult Register()
        {
            var model = _scope.Resolve<RegisterModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var result = await model.Register();
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Login");
            //}

            //ModelState.AddModelError("", result.ErrorMessage);
            return View(model);
        }   
    }
}
