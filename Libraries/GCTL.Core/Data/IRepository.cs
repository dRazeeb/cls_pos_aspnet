using System.Linq.Expressions;

namespace GCTL.Core.Data
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Add(IEnumerable<T> entities);
        IQueryable<T> All();
        IQueryable<T> FindBy(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(object id);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }
}