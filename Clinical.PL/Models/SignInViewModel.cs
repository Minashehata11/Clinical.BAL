using System.ComponentModel.DataAnnotations;

namespace Clinical.PL.Models
{
	public class SignInViewModel
	{
		[EmailAddress]
		public string Email { get; set; }

        public bool Remeberme { get; set; }

        public string Password { get; set; }
	}
}
