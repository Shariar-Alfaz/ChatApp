using System.ComponentModel.DataAnnotations;

namespace ChatApp.Web.Models.Auth
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression("[a-zA-z]+",ErrorMessage ="First name is not valid")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("[a-zA-z]+", ErrorMessage = "Last name is not valid")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Email is not valid")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("[0-9]{11}")]
        public string ContactNumber { get; set; }
        [Required]
        [MinLength(8,ErrorMessage ="Password must be 8 characters long")]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Password does not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        [RegularExpression("[a-zA-z]+", ErrorMessage = "Country name is not valid")]
        public string Country { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        [RegularExpression("[a-zA-z]+", ErrorMessage = "Provience name is not valid")]
        public string Provience { get; set; }
    }
}
