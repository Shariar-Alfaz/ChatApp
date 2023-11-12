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
           var model = _scope.Resolve<LoginModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email or password is not valid.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ModelState.AddModelError("Email", "Email or password is not valid.");
            }
            return View(model);
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
                    Province = model.Province,
                    PhoneNumber = model.ContactNumber,
                    RegistrationDate = DateTime.Now
                };
                var isEmailExist = await _userManager.FindByEmailAsync(model.Email);
                if (isEmailExist != null)
                {
                    ModelState.AddModelError("Email", "Email already exist");
                    return View(model);
                }
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
                    return RedirectToAction("confirmEmailMessage", "auth",
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
                return RedirectToAction("index", "home");
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("success","auth",new {userId = user.Id});
            }
            return RedirectToAction("index", "home");
        }

        public IActionResult ConfirmEmailMessage(string userName, string email)
        {
            var model = _scope.Resolve<RegistrationMessageModel>();
            model.Name = userName;
            model.Email = email;
            return View(model);
        }

        public IActionResult Success(Guid userId)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            var model = _scope.Resolve<RegistrationMessageModel>();
            model.Name = $"{user!.FirstName} {user.LastName}";
            model.Email = user.Email!;
            return View(model);
        }   

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }

        public IActionResult ForgotPassword()
        {
            var model = _scope.Resolve<ForgotPasswordModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Account does not exist");
                return View(model);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = Url.Action("resetpassword", "auth", new { userId = user.Id, token = token }, Request.Scheme);
            await _emailMessageService.SendForgotPasswordEmailAsync(
                               user.Email, $"{user.FirstName} {user.LastName}", callbackUrl!);
            return RedirectToAction("forgotPasswordMessage", "auth",
                               new { userName = $"{user.FirstName} {user.LastName}", email = user.Email });
        }

        public IActionResult ForgotPasswordMessage(string userName, string email)
        {
            var model = _scope.Resolve<RegistrationMessageModel>();
            model.Name = userName;
            model.Email = email;
            return View(model);
        }

        public async Task<IActionResult> ResetPassword(Guid userId, string token)
        {
            if (userId == Guid.Empty || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("index", "home");
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var model = _scope.Resolve<ResetPasswordModel>();
            model.UserId = userId;
            model.Token = token;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("resetpasswordsuccess");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }
    }
}
