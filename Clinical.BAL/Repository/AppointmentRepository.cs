using Clinical.BAL.Interfaces;
using Clinical.DAL.Context;
using Clinical.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAppointmentByDoctorIdAsync(string id)
             =>  await _context.Appointments.Include(app => app.Doctor).Include(app => app.Patient).
            ThenInclude(x => x.User).Where(reser => reser.DoctorId == id && reser.IsChecked == false).ToListAsync();

        public async Task<List<Appointment>> GetAppointmentByPatientIdAsync(string id)
        => await _context.Appointments.Include(app=>app.Patient).Include(app=>app.Doctor).
            ThenInclude(x=>x.User).Where(reser=>reser.PatientId==id && reser.IsChecked == false).ToListAsync();

        public async Task<Appointment> GetAppointmentWithIncudeByIdAsync(int? id)
        => await _context.Appointments.Include(a => a.Patient).ThenInclude(x=>x.User).FirstOrDefaultAsync(x => x.AppointemntId == id);
    }
}
