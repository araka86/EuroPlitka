using EuroPlitka.Models;
using EuroPlitka_DataAccess.Data;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_DataAccess.Repository.IReposotory;

namespace EuroPlitka.Controllers
{
   

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
       

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
           _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Details(int id)
        {
            //take session
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
            }

            DetailsVM DetailsVM = new DetailsVM()
            {
                Product = await _productRepository.FirstOrDefault(u => u.Id == id, includeProperties: "Category,ProductType"),
                ExistInCart = false
            };

            //check item session
            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                    DetailsVM.ExistInCart = true;
            }

            return View(DetailsVM);
        }

        //Add Datail to cart
        [HttpPost, ActionName("Details")]
        public async Task<IActionResult> DetailsPost(int id, DetailsVM detailsVM)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id, Sqft = detailsVM.Product.TempSqFt });
            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartList);
            TempData[WebConstanta.Success] = "Product Add to cart successfully";


          

            var productCat = await _productRepository.FirstOrDefault(x => x.Id == shoppingCartList.LastOrDefault().ProductId);
            return RedirectToAction("Index", "CategoryMenu", new { id = productCat.CategoryId });

        }












        //Delete Detail
        public async Task<IActionResult> RemoveFromCart(int id)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);

            if (itemToRemove != null)
                shoppingCartList.Remove(itemToRemove);




            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartList);

            TempData[WebConstanta.Success] = "Product remote to cart successfully";

            //return RedirectToAction(nameof(Index));
            if(shoppingCartList.Count > 0)
            {
                var productCat = await _productRepository.FirstOrDefault(x => x.Id == shoppingCartList.LastOrDefault().ProductId);
                return RedirectToAction("Index", "CategoryMenu", new { id = productCat.CategoryId });
            }


            var productCatLast = await _productRepository.FirstOrDefault(x => x.Id == itemToRemove.ProductId);
            return RedirectToAction("Index", "CategoryMenu", new { id = productCatLast.CategoryId });
        }











        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}