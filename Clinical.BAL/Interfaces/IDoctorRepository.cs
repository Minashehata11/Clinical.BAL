using Clinical.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Interfaces
{
    public interface IDoctorRepository:IGenericRepository<Doctor>
    {
        public List<Doctor> Search(string name);
        public List<Doctor> GetAllWithInclude();
        public Doctor GetByIdWithInclude(string id);

        

    }
}
