using EuroPlitka.Models;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_DataAccess.Repository.IRepository;
using System.Security.Claims;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace EuroPlitka.Controllers
{
    public class HomeController : Controller
    {
      
        private readonly IProductRepository _productRepository;
        private readonly INewsRepositoriy _newsRepositoriy;
        private readonly IBasketRepo _basketRepo;
     


        public HomeController(IProductRepository productRepository,
            INewsRepositoriy newsRepositoriy,
            IBasketRepo basketRepo)
        {
            _productRepository = productRepository;
            _newsRepositoriy = newsRepositoriy;
            _basketRepo = basketRepo;         
        }

      

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {

            var langue1 = CookieRequestCultureProvider.DefaultCookieName;
            var langue2 = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
            var langue3 = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };






            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
        }



        public async Task<IActionResult> Index()
        {

            var langue1 = CookieRequestCultureProvider.DefaultCookieName;
            var langue2 = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en"));
            var langue3 = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };



            var productNews = new NewsProducstHomeVM()
            {
                News = await _newsRepositoriy.GetAll(x => x.IsMainMenu == true, orderBy: y => y.OrderByDescending(c => c.DateTime)),
                Products =  _productRepository.GetAllProduct().GetAwaiter().GetResult().Take(4)
            };

            var news = await _newsRepositoriy.GetAll(x => x.IsMainMenu == true, orderBy: y => y.OrderByDescending(c => c.DateTime));

            await _newsRepositoriy.ChkMarkItem(productNews.News);







            //  var claimsIndentity = (ClaimsIdentity)User.Identity;
            //   var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);
              var usr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nameUser = User.Identity.Name;

            if (usr != null)
            {

                var getbasketUser = _basketRepo.GetAll(x => x.CreatedByUserId == usr).Result;
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




            return this.View(productNews);
          //  return this.View(news);
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

            if(DetailsVM.Product == null)
            {
                TempData[WebConstanta.Error] = "Prodoct  not exist!!";
                return RedirectToAction("Index");
            }
              


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
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Any())
            {
                shoppingCartList =  HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
            }

            shoppingCartList.Add(new ShoppingCart { ProductId = product.Id, Sqft = product.TempSqFt });

            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartList);

            TempData[WebConstanta.Success] = "Product Add to cart successfully";



            var productCat = await _productRepository.FirstOrDefault(x => x.Id == shoppingCartList.LastOrDefault().ProductId);


            if (User.Identity.IsAuthenticated)
            {

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

            }
           



            return RedirectToAction("Index", "CategoryMenu", new { id = productCat.CategoryId });
        }

        //Delete Detail
        public async Task<IActionResult> RemoveFromCart(int id)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
            }
            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
                shoppingCartList.Remove(itemToRemove);


            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartList);



            if (User.Identity.IsAuthenticated)
            {
                var claimsIndentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);


                //Remove Basket
                var basetUser = await _basketRepo.FirstOrDefault(x => x.ProductId == id && x.CreatedByUserId == claim.Value);
                _basketRepo.Delete(basetUser);
            }


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