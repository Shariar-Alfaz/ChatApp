using System.ComponentModel.DataAnnotations;

namespace ChatApp.Web.Models.Auth
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
    }
}
