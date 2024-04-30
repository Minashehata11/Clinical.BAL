using Clinical.DAL.Context;
using Clinical.DAL.Entities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinical.BAL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        

        public IEnumerable<T> GetAll();
        public T GetById(string? Id);

        public void Add(T entity);

        public void Delete(T entity);

        public void Update(T entity);
    }
}
