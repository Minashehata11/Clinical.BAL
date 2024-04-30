using Clinical.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.DAL.Entities
{
    public class Doctor:BaseEntity
    {
        [Key]

        public string DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public ApplicationUser User { get; set; }

        public string Specialty { get; set; }

        public string? Image { get; set; }
    }
}
