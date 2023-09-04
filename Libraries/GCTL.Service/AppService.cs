using GCTL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Service
{
    public class AppService<T> where T : class
    {
        private readonly IRepository<T> repository;

        public AppService(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public List<T> GetAll()
        {
            return repository.GetAll().ToList();
        }
        public T GetByCode(string code)
        {
            return repository.GetById(code);
        }

        public T Add(T entity)
        {
            return repository.Add(entity);
        }

        public void Update(T entity)
        {
            repository.Update(entity);
        }

        public bool Delete(string code)
        {
            var entity = GetByCode(code);
            if (entity != null)
            {
                Delete(entity);
                return true;
            }
            return false;
        }

        public void Delete(T entity)
        {
            repository.Delete(entity);
        }
    }
}