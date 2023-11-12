using System.ComponentModel.DataAnnotations;

namespace ChatApp.Web.Models.Auth
{
    public class ResetPasswordModel
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters long")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
