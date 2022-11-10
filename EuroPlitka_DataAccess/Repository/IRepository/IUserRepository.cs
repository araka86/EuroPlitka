using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;

namespace EuroPlitka_DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<AplicationUser>
    {
        void Update(AplicationUser user);

        Task<AplicationUser> GetUserById(string id);

    }
}
