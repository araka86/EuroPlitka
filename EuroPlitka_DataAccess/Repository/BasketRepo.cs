using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Repository
{
    public class BasketRepo : Repository<Basket>, IBasketRepo
    {
        public BasketRepo(EuroPlitkaDbContext db) : base(db)
        {
        }

        public bool AddRange(IEnumerable<Basket> items)
        {
            dbSet.AddRange(items);
            return Save();
        }

        public bool RemoveRange(IEnumerable<Basket> items)
        {          
            dbSet.RemoveRange(items);
            return Save();
        }
    }
}
