using EuroPlitka_Model;
using System.Linq.Expressions;

namespace EuroPlitka_DataAccess.Repository.IReposotory
{
    public interface IRepository<T> where T : class
    {

        Task<T> Find(int id);

       Task<IEnumerable<T>> GetAll(
                            Expression<Func<T, bool>>? filter = null,                   //for object
                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, //orderBy enumerable functions
                            string? includeProperties = null,                           //for prperty
                            bool isTracking = true
                            );


        Task<IEnumerable<T>> GetAllFilter(
                           string? includeProperties = null,                           
                           Expression<Func<T, bool>>? filter = null,                   
                           bool isTracking = true
                           );



        Task<T> FirstOrDefault(
            Expression<Func<T, bool>>? filter = null, 
            string? includeProperties = null, 
            bool isTracking = true
            );

        //void Add(T entity);
        //void Remove (T entity);

        //void RemoveRange(IEnumerable<T> entity);
        //void Save();


        bool Add(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        bool Save();
     
    }
}
