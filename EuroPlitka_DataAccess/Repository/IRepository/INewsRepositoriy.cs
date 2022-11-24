using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository.IRepository
{
    public interface INewsRepositoriy : IRepository<News>
    {

       public Task<IEnumerable<News>> MarkItem(IEnumerable<News> items);
       public bool UpdateRange(IEnumerable<News> items);
    }
}
