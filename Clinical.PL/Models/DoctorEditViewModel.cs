using System.ComponentModel.DataAnnotations;

namespace Clinical.PL.Models
{
	public class DoctorEditViewModel
	{
		[EmailAddress]
		public string Email { get; set; }
		public string Specialty { get; set; }
		public IFormFile? File { get; set; }

        public string? Image { get; set; }
        public string UserName { get; set; }
    }
}
