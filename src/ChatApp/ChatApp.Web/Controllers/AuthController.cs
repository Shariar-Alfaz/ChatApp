using Autofac;
using ChatApp.Application.Feature.Services;
using ChatApp.Persistence.Membership;
using ChatApp.Web.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailMessageService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthController(
            ILifetimeScope scope,
            ILogger<AuthController> logger,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _scope = scope;
            _logger = logger;
            _userManager = userManager;
            _emailMessageService = emailService;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Street = model.Street,
                    Country = model.Country,
                    Provience = model.Provience,
                    PhoneNumber = model.ContactNumber,
                    RegistrationDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync("User");
                    if (role == null)
                    {
                        role = new ApplicationRole
                        {
                            Name = "User"
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, role.Name!);
                    var claims = new List<Claim>
                    {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, role.Name!),
                        new Claim("RegistrationDate", user.RegistrationDate.ToString()),
                    };
                    await _userManager.AddClaimsAsync(user, claims);
                    var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    emailConfirmToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmToken));
                    var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = emailConfirmToken }, Request.Scheme);
                    await _emailMessageService.SendRegistrationConfirmationEmailAsync(
                        user.Email, $"{user.FirstName} {user.LastName}", callbackUrl!);
                    return RedirectToAction("ConfirmEmailMessage", "Auth",
                        new { userName = $"{user.FirstName} {user.LastName}", email = user.Email });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user");
                return View(model);
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            if (userId == Guid.Empty || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ConfirmEmailMessage(string userName, string email)
        {
            var model = _scope.Resolve<RegistrationMessageModel>();
            model.Name = userName;
            model.Email = email;
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
