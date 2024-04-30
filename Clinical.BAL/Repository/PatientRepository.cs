using Clinical.BAL.Interfaces;
using Clinical.DAL.Context;
using Clinical.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Repository
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
		private readonly ApplicationDbContext _context;

		public PatientRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
		

		public Patient GetByIdWithInclude(string id)
		=> _context.Patients.Include(x => x.User).FirstOrDefault(x => x.PatientId == id);

		public List<Patient> GetAllWithInclude()
		=> _context.Patients.Include(x => x.User).ToList();
		public List<Patient> Search(string name)
		{
			return _context.Patients.Include(x => x.User)
				.Where(patient => patient.User.UserName.Trim().ToLower().Contains(name.Trim().ToLower())).ToList();
		}
	}
}
