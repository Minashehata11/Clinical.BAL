using Clinical.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Interfaces
{
    public interface IAppointmentRepository:IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentByPatientIdAsync(string id);
        Task<List<Appointment>> GetAppointmentByDoctorIdAsync(string id);
        Task<Appointment> GetAppointmentWithIncudeByIdAsync(int? id);

    }
}
