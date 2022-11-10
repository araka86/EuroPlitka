using System.Linq.Expressions;

namespace EuroPlitka_DataAccess.Repository.IReposotory
{
    public interface IRepository<T> where T : class
    {

        T Find(int id);

        IEnumerable<T> GetAll(
                            Expression<Func<T, bool>>? filter = null,                   //for object
                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, //orderBy enumerable functions
                            string? includeProperties = null,                           //for prperty
                            bool isTracking = true
                            );

        T FirstOrDefault(
            Expression<Func<T, bool>>? filter = null, 
            string? includeProperties = null, 
            bool isTracking = true
            );

        void Add(T entity);
        void Remove (T entity);

        void RemoveRange(IEnumerable<T> entity);

      

        void Save();
    }
}
