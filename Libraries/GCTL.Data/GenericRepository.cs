using GCTL.Core.Data;
using GCTL.Data.Models;
using System.Linq.Expressions;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace GCTL.Data
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<T> All()
        {
            return context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Where(expression);
        }

        public IQueryable<T> Get(FormattableString sql)
        {
            return context.Set<T>().FromSqlInterpolated(sql);
        }

        public T GetById(object id)
        {
            return context.Set<T>().Find(id);
        }

        public T Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public void Add(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            //    var state = context.Entry(entity).State;


            // context.Entry(entity).State = EntityState.Modified;
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public void Update(IEnumerable<T> entities)
        {
            context.Set<T>().UpdateRange(entities);
            context.SaveChanges();
        }

        public void Delete(object id)
        {
            var entity = GetById(id);
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
            context.SaveChanges();
        }
    }
}
