﻿using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository
{
    public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(EuroPlitkaDbContext db) : base(db)
        {
        }

        public void Update(ProductType obj)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
                objFromDb.Name = obj.Name;

        }
    }
}
