using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EuroPlitka.Controllers
{


    public class CategoryViewComponent : ViewComponent
    {
        private readonly EuroPlitkaDbContext _applicationDbContext;

        public CategoryViewComponent(EuroPlitkaDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _applicationDbContext.Categorys.ToListAsync();
            return View(result);
        }

    }




    public class CategoryMenuController : Controller
    {
      
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryMenuController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
          
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }



        public async Task<IActionResult> Index(int id, int page = 1, int pageSize = 3, bool allResultPage = false)
        {
            CategoryMenuVM homeVm;

            //take session
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
            }


            if (!allResultPage)
            {
                if (page < 1 || pageSize < 1)
                {
                    return NotFound();
                }

                var countToConditionProduct = await _productRepository.GetSliceAsync((page - 1) * pageSize, pageSize, id);
                var countAllCurrentCategory = await _productRepository.GetProductCategory(u => u.CategoryId == id, includeProperties: "Category,ProductType");
                homeVm = new CategoryMenuVM()
                {
                    Products = await _productRepository.GetAll(includeProperties: "Category,ProductType"),
                    Categories = await _categoryRepository.GetAll(),
                    ProductsCat = await _productRepository.GetProductCategory(u => u.CategoryId == id, includeProperties: "Category,ProductType"),
                    CategoryProduct = await _categoryRepository.FirstOrDefault(x => x.Id == id),

                    ConditionProducts = countToConditionProduct,
                    Page = page,
                    PageSize = pageSize,
                    TotalCountAllCurrentCategory = countAllCurrentCategory.Count(),
                    TotalPages = (int)Math.Ceiling(countAllCurrentCategory.Count() / (double)pageSize),
                    AllPage = allResultPage


                };
            }
            else
            {

                homeVm = new CategoryMenuVM()
                {
                    Products = await _productRepository.GetAll(includeProperties: "Category,ProductType"),
                    Categories = await _categoryRepository.GetAll(),
                    ProductsCat = await _productRepository.GetProductCategory(u => u.CategoryId == id, includeProperties: "Category,ProductType"),
                    CategoryProduct = await _categoryRepository.FirstOrDefault(x => x.Id == id),
                    AllPage = allResultPage

                };

            }


            //check item session


            foreach (var itemProd in homeVm.Products)
            {
                foreach (var ses in shoppingCartList)
                {
                    if (ses.ProductId == itemProd.Id)
                    {
                        itemProd.ExistInCart = true;
                    }
                }
            }



            return View(homeVm);
        }

        public async Task<IActionResult> AllCategory()
        {
            return View();
        }




    }
}
