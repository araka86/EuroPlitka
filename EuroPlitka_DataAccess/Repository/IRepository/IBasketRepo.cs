using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository.IRepository
{
    public interface IBasketRepo : IRepository<Basket>
    {

        public bool RemoveRange(IEnumerable<Basket> items);
        public bool AddRange(IEnumerable<Basket> items);
    }



}
