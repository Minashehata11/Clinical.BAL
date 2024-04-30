using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Interfaces
{
    public interface IUnitWork
    {
        public IDoctorRepository DoctorRepository { get; set; }
        public IPatientRepository PatientRepository { get; set; }
        public IAppointmentRepository appointmentRepository { get; set; }
        public int Commit();
    }
}
