using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Repository
{
    public class ResourcesRepo : Repository<Resource>, IResourcesRepo
    {
        private readonly EuroPlitkaDbContext _db;
        public ResourcesRepo(EuroPlitkaDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SelectListItem>> DropdownLists(string obj)
        {
            //DropDownList
            if (obj == WebConstanta.ViewName)
            {
                return _db.Views.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            if (obj == WebConstanta.Pagefille)
            {
                return _db.Pagefilles.Select(u => new SelectListItem
                {
                    Text = u.Filename,
                    Value = u.Id.ToString()
                });
            }


            return null;
        }
    }
}
