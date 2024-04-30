using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.DAL.Entities
{
    public class Appointment:BaseEntity
    {
     
        public int AppointemntId { get; set; }

        public Doctor Doctor { get; set; }

        public string DoctorId { get; set; }

        public Patient Patient { get; set; }

        public string PatientId { get; set; }
        [Required]
        public DateTime DateReserve { get; set; }

        public string ReasonForVisit { get; set; }

        public bool IsChecked { get; set; } = false;

    }
}
