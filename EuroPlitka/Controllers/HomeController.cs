using EuroPlitka.Models;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_DataAccess.Repository.IRepository;
using Syncfusion.XlsIO;
using EuroPlitka_DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EuroPlitka.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AplicationUser> _userManager;
        private readonly IProductRepository _productRepository;
        private readonly INewsRepositoriy _newsRepositoriy;
        private readonly EuroPlitkaDbContext _euroPlitkaDbContext;
        private readonly IBasketRepo _basketRepo;



        public HomeController(IProductRepository productRepository, 
            INewsRepositoriy newsRepositoriy, 
            EuroPlitkaDbContext euroPlitkaDbContext,
            IBasketRepo basketRepo,
            UserManager<AplicationUser> userManager)
        {

            _productRepository = productRepository;
            _newsRepositoriy = newsRepositoriy;
            _euroPlitkaDbContext = euroPlitkaDbContext;
            _basketRepo = basketRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
           
        

            var prod = await _productRepository.GetAll(includeProperties: "Category,ProductType");



            var productNews = new NewsProducstHomeVM()
            {
              News = await _newsRepositoriy.GetAll(x => x.IsMainMenu == true, orderBy: y => y.OrderByDescending(c => c.DateTime)),
              Products = prod.Take(4)

            };
            
           // productNews.News = await _newsRepositoriy.MarkItem(productNews.News);
           await  _newsRepositoriy.ChkMarkItem(productNews.News);

           



            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {

                var getbasketUser = _basketRepo.GetAll(x => x.CreatedByUserId == claim.Value).Result;

                if (getbasketUser != null && getbasketUser.Any())
                {
                    List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

                    if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
                       HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Any())
                    {
                        shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
                    }
                    else
                    {
                        foreach (var item in getbasketUser)
                        {
                            shoppingCartList.Add(new ShoppingCart { ProductId = (int)item.ProductId, Sqft = item.Sqft });
                        }

                        HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartList);

                    }
                  

                }
            }




            return View(productNews);
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
      
        public async Task<IActionResult> DetailsPost(Product product)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
            }

            shoppingCartList.Add(new ShoppingCart { ProductId = product.Id, Sqft = product.TempSqFt });

            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartList);

            TempData[WebConstanta.Success] = "Product Add to cart successfully";
            var productCat = await _productRepository.FirstOrDefault(x => x.Id == shoppingCartList.LastOrDefault().ProductId);


            //Add to Basket(Backup)
            //get id user
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);

            Basket basket = new Basket()
            {
                CreatedByUserId = claim.Value,
                Data = DateTime.Now,
                ProductId = productCat.Id,
                Sqft = product.TempSqFt
            };

           
            _basketRepo.Add(basket);
            _basketRepo.Save();



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
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);

            //Remove Basket
            var basetUser = await _basketRepo.FirstOrDefault(x=>x.ProductId == id && x.CreatedByUserId == claim.Value );
            _basketRepo.Delete(basetUser);



            TempData[WebConstanta.Success] = "Product remote to cart successfully";
            if (shoppingCartList.Count > 0)
            {
                var productCat = await _productRepository.FirstOrDefault(x => x.Id == shoppingCartList.LastOrDefault().ProductId);
                return RedirectToAction("Index", "CategoryMenu", new { id = productCat.CategoryId });
            }
            var productCatLast = await _productRepository.FirstOrDefault(x => x.Id == itemToRemove.ProductId);
            return RedirectToAction("Index", "CategoryMenu", new { id = productCatLast.CategoryId });
        }

        public ActionResult Privacy() => View();
        public ActionResult About() => View();
        public ActionResult Modal(int id) => PartialView();

        [HttpGet]
        public async Task<IActionResult> SearchPartialModal(string Name) => PartialView(await _productRepository.GetAll(x => x.Name.Contains(Name)));









        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}