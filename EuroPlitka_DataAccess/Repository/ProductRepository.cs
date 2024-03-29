﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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








        public  async Task<IEnumerable<SelectListItem>> GetAllDropdownList(string obj)
        {
            //DropDownList
           if(obj == WebConstanta.CategoryName)
            {
                return  _db.Categorys.Select(u => new SelectListItem
                {
                    Text = u.NameUa,
                    Value = u.Id.ToString()
                });
            }
            if (obj == WebConstanta.ProductTypeName)
            {
                return  _db.ProductTypes.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }


            return null;
        }




        public async Task<IEnumerable<SelectListItem>> GetAllDropdownListEng(string obj)
        {
            //DropDownList
            if (obj == WebConstanta.CategoryName)
            {
                return _db.Categorys.Select(u => new SelectListItem
                {
                    Text = u.NameEng,
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

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await GetAll(includeProperties: "Category,ProductType");
        }


        public async Task<int> GetCountAsync() => await _db.Product.CountAsync();
        

        public async Task<IEnumerable<Product>> GetProductCategory(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool isTracking = true)
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
            return  await query.ToListAsync();
        }









        public async Task<IEnumerable<Product>> GetSliceAsync(int offset, int size, int idCat)
        {
            return await _db.Product.Include(i => i.Category)
                .Include(x=>x.ProductType).Where(y=>y.CategoryId ==idCat)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }


        public bool RemoveRange(IEnumerable<Product> items)
        {
            dbSet.RemoveRange(items);
            return Save();
        }



        public async Task<IEnumerable<Product>> GetAllTest()
        {
            return await _db.Product.ToListAsync();
        }

    }
}
