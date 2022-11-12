using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly EuroPlitkaDbContext _db;

        public CategoryRepository(EuroPlitkaDbContext db):base(db)
        {
            _db = db;
        }

        public async void Update(Category obj)
        {
           var objFromDb = await FirstOrDefault(u=>u.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.DisplayOrder = obj.DisplayOrder;
            }
        }

        
    }
}
