using System.Linq.Expressions;
using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace EuroPlitka_DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);

       Task<IEnumerable<SelectListItem>> GetAllDropdownList(string obj);
      Task<IEnumerable<Product>> GetProductCategory(
            Expression<Func<Product, bool>>? filter = null,
            string? includeProperties = null,
            bool isTracking = true);



          Task<IEnumerable<Product>> GetSliceAsync(int offset, int size, int idCat);


    }

  



}
