using System.ComponentModel.DataAnnotations;

namespace Clinical.PL.Models
{
    public class SignUpViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8,ErrorMessage =" Password Must be Greater Than 8 Character")]

        public string Password { get; set; }

        [MinLength(8, ErrorMessage = " Password Must be Greater Than 8 Character")]
        [Compare(nameof(Password), ErrorMessage = "Dont match")]
        public string ConfirmedPassword { get; set; }

        public string Address { get; set; }
    }
}
