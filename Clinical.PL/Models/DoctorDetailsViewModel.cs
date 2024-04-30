using System.ComponentModel.DataAnnotations;

namespace Clinical.PL.Models
{
	public class DoctorDetailsViewModel
	{
        public string Email { get; set; }
        public string Name { get; set; }
		public string Specialty { get; set; }
        public string Image { get; set; }
       
    }
}
