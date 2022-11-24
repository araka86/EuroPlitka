using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository
{
    public class NewsRepositoriy : Repository<News>, INewsRepositoriy
    {
        public NewsRepositoriy(EuroPlitkaDbContext db) : base(db)
        {
        }
    }
}
