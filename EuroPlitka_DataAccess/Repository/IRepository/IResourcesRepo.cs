using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EuroPlitka_DataAccess.Repository.IRepository
{
    public interface IResourcesRepo: IRepository<Resource>
    {
        Task<IEnumerable<SelectListItem>> DropdownLists(string obj);
    }
}
