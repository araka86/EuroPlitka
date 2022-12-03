using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Repository.IRepository
{
    public interface IBasketRepo : IRepository<Basket>
    {



        public bool RemoveRange(IEnumerable<Basket> items);
    }



}
