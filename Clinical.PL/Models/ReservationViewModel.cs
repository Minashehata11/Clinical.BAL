using System.ComponentModel.DataAnnotations;

namespace Clinical.PL.Models
{
    public class ReservationViewModel
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public DateTime DateReserve { get; set; }
        public string ReasonForVisit { get; set; }
    }
}
