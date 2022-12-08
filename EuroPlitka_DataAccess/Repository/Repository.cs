using EuroPlitka_DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using EuroPlitka_DataAccess.Repository.IReposotory;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EuroPlitka_DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EuroPlitkaDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(EuroPlitkaDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        //public void Add(T entity)
        //{
        //    dbSet.Add(entity);
        //}

       

        public async Task<T> Find(int id)
        {
            return  dbSet.Find(id);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>>? filter = null, 
                                string? includeProperties = null,
                                bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includePror in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePror);
                }
            }
           
            if (!isTracking)
            {
                query = query.AsNoTracking(); // don't tracking query
            }
            return  query.FirstOrDefault();
        }

        public  async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null,
                                    Func<IQueryable<T>,
                                    IOrderedQueryable<T>>? orderBy = null,
                                    string? includeProperties = null,
                                    bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includePror in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePror);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking(); 
            }
            return query.ToList();

           
        }

        //public void Remove(T entity)
        //{
        //    dbSet.Remove(entity);
        //}

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);

        }

        //public void Save()
        //{
        //    _db.SaveChanges();
        //}





        public bool Add(T entity)
        {
            _db.Add(entity);
            return Save();
        }

        public bool Delete(T entity)
        {
            _db.Remove(entity);
            return Save();
        }



        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0;
        }

        public bool Update(T entity)
        {
           _db.Update(entity);
          return  Save();
        }

        public async Task<IEnumerable<T>> GetAllFilter(string? includeProperties = null, Expression<Func<T, bool>>? filter = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (includeProperties != null)
            {
                foreach (var includePror in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includePror);
                }
            }
         
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }


            return query.ToList();
        }
    }
}
