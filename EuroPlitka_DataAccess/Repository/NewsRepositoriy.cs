using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using Microsoft.EntityFrameworkCore;

namespace EuroPlitka_DataAccess.Repository
{
    public class NewsRepositoriy : Repository<News>, INewsRepositoriy
    {

        private readonly EuroPlitkaDbContext _db;
        public NewsRepositoriy(EuroPlitkaDbContext db) : base(db)
        {
           
        }

        public async Task<IEnumerable<News>> GetAllTest()
        {
            return await _db.News.ToListAsync();
        }

        public async Task<IEnumerable<News>> MarkItem(IEnumerable<News> items)
        {
           
           foreach (var item in items)
            {
                item.IsFirst = true;
                break;
           }
                


            return items;
        }

        public async Task<INewsRepositoriy> ChkMarkItem(IEnumerable<News> items)
        {
            foreach (var item in items)
            {
                item.IsFirst = true;
                break;
            }

            return await Task.FromResult(this);
        }


        public bool UpdateRange(IEnumerable<News> items)
        {
            foreach (var item in items)
            {
                if (item.checkedState == "on")
                {
                    item.IsMainMenu = true;
                }
            }
            dbSet.UpdateRange(items);
            
            return Save();
        }
    }
}
