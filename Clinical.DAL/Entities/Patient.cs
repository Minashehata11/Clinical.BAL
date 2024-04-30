using Clinical.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.DAL.Entities
{
    public class Patient:BaseEntity
    {
        [Key]

        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public ApplicationUser User { get; set; }

        public string? Address { get; set; }

    }
}
