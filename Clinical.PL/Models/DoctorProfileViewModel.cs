namespace Clinical.PL.Models
{
    public class DoctorProfileViewModel
    {
        public int AppointemntId { get; set; }
        public string patientName { get; set; }
        public string PatientEmail { get; set; }
        public string ReasonForVisit { get; set; }
        public string Address { get; set; }
        public bool IsChecked { get; set; }
        public DateTime ReservationTime { get; set; }
    }
}
