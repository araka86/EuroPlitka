using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository
{
    public class UserRepository : Repository<AplicationUser>, IUserRepository
    {
        private readonly EuroPlitkaDbContext _db;

        public UserRepository(EuroPlitkaDbContext db) : base(db)
        {
            _db = db;
        }

       
    }
}
