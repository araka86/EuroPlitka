using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Repository
{
    public class UserRepository : Repository<AplicationUser>, IUserRepository
    {
        private readonly EuroPlitkaDbContext _db;

        public UserRepository(EuroPlitkaDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<AplicationUser> GetUserById(string id)
        {
            return await _db.AplicationUser.FindAsync(id);
        }

        public void Update(AplicationUser user)
        {
            _db.Update(user);
        }
    }
}
