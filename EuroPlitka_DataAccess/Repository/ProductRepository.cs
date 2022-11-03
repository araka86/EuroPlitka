using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using EuroPlitka_DataAccess.Repository;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_DataAccess.Data;
using EuroPlitka_Services;

namespace EuroPlitka_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly EuroPlitkaDbContext _db;

        public ProductRepository(EuroPlitkaDbContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            //DropDownList
           if(obj == WebConstanta.CategoryName)
            {
                return _db.Categorys.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            if (obj == WebConstanta.ProductTypeName)
            {
                return _db.ProductTypes.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }


            return null;
        }

        public IEnumerable<Product> GetProductCategory(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool isTracking = true)
        {
            IQueryable<Product> query = dbSet;
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
                query = query.AsNoTracking();
            }
            return query.ToList();
        }

        public void Update(Product obj)
        {
         
            _db.Product.Update(obj);
        }

        
    }
}
