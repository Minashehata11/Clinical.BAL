using Clinical.BAL.Interfaces;
using Clinical.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Repository
{
    public class UnitWork : IUnitWork
    {
        private readonly ApplicationDbContext _context;

        public IDoctorRepository DoctorRepository { get ; set; }
        public IPatientRepository PatientRepository { get ; set ; }
        public IAppointmentRepository appointmentRepository { get ; set ; }

        public UnitWork(ApplicationDbContext context)
        {
            DoctorRepository=new DoctorRepository(context);
            PatientRepository=new PatientRepository(context);
            appointmentRepository=new AppointmentRepository(context);   
            _context = context;
        }
        public int Commit()
        {
           return _context.SaveChanges();
        }
    }
}
