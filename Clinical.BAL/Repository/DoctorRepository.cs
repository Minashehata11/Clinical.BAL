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
    public class DoctorRepository:GenericRepository<Doctor>,IDoctorRepository
    {
        private readonly ApplicationDbContext _context;  

        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Doctor> GetAllWithInclude()
        => _context.Doctors.Include(x=>x.User).ToList();

        public Doctor GetByIdWithInclude(string id)
        => _context.Doctors.Include(x => x.User).FirstOrDefault(x => x.DoctorId == id);

		public List<Doctor> Search(string name)
        {
            return _context.Doctors.Include(x => x.User)
                .Where(doctor => doctor.User.UserName.Trim().ToLower().Contains(name.Trim().ToLower())).ToList();
        }
    }
}
