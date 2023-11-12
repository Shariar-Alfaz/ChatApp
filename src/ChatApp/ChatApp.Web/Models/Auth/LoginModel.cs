using System.ComponentModel.DataAnnotations;

namespace ChatApp.Web.Models.Auth
{
    public class LoginModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
