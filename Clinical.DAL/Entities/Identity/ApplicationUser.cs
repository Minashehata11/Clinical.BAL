using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.DAL.Entities.Identity
{
    public enum Gender
    {
        Male,
        Female
    }
    public class ApplicationUser:IdentityUser
    {
        [StringLength(2,ErrorMessage ="Age Less Than that")]
        public  byte Age { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public Gender Gender { get; set; }
    }
}
