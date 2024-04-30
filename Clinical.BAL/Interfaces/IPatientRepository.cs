using Clinical.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Interfaces
{
    public interface IPatientRepository:IGenericRepository<Patient>
    {
		public Patient GetByIdWithInclude(string id);
		public List<Patient> GetAllWithInclude();
		public List<Patient> Search(string name);


	}
}
