using Clinical.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Clinical.PL.Models
{
    public class DoctorCreateViewModel
    {

        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = " Password Must be Greater Than 8 Character")]

        public string Password { get; set; }

        [MinLength(8, ErrorMessage = " Password Must be Greater Than 8 Character")]
        [Compare(nameof(Password), ErrorMessage = "Dont match")]
        public string ConfirmedPassword { get; set; }

        public string Specialty { get; set; }

        //public string? image { get; set; }

		// public  MyProperty { get; set; }
		public IFormFile File { get; set; }
	}
}
