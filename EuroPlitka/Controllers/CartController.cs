using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EuroPlitka.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IBasketRepo _basketRepo;
        private readonly EuroPlitkaDbContext _euroPlitkaDbContext;

        [BindProperty]
        public ProductUserViewModel productuserViewModel { get; set; }

        public CartController(IProductRepository productRepository,
            IOrderDetailRepository orderDetailRepository,
            IOrderHeaderRepository orderHeaderRepository,
            EuroPlitkaDbContext euroPlitkaDbContext,
            IBasketRepo basketRepo
            )
        {
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderHeaderRepository = orderHeaderRepository;
            _euroPlitkaDbContext = euroPlitkaDbContext;
            _basketRepo = basketRepo;
        }



        //CART INDEX
        public async Task<IActionResult> Index()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();


            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCarts = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).ToList();
            }
            List<int> productCart = shoppingCarts.Select(x => x.ProductId).ToList();

            IEnumerable<Product> productsListTemp = await _productRepository.GetAll(u => productCart.Contains(u.Id)); //временный список
            IList<Product> productsList = new List<Product>(); //финальный список для передачи во View

            foreach (var cartObj in shoppingCarts)
            {
                Product prodtemp = productsListTemp.FirstOrDefault(u => u.Id == cartObj.ProductId); //get object from  prodtemp
                prodtemp.TempSqFt = cartObj.Sqft;
                productsList.Add(prodtemp);
            }
            return View(productsList);
        }


        //continue button
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> prodList)
        {

            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            foreach (Product prod in prodList)
            {
                shoppingCartsList.Add(new ShoppingCart
                {
                    ProductId = prod.Id,
                    Sqft = prod.TempSqFt
                });
            }
            //set value for  current session
            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartsList);
            return RedirectToAction(nameof(Summary));
        }
        //continue button 2
        public async Task<IActionResult> Summary()
        {

            AplicationUser aplicationUser;
            // заполнения информации о пользователе в корзине
            if (User.IsInRole(WebConstanta.AdminRole))
            {
                //проверка значений для сеанаса

                if (HttpContext.Session.Get<int>(WebConstanta.SessionInquiryId) != 0) //if not equal to 0, it means that some request is being processed
                {
                    OrderHeader orderHeader = await _orderHeaderRepository.FirstOrDefault(u => u.Id == HttpContext.Session.Get<int>(WebConstanta.SessionInquiryId));
                    //filling the cart based on the current request
                    aplicationUser = new AplicationUser()
                    {
                        Email = orderHeader.Email,
                        FullName = orderHeader.FullName,
                        PhoneNumber = orderHeader.PhoneNumber
                    };
                }
                else
                {
                    //admin places an order for a customer who just came to the store without using the site
                    aplicationUser = new AplicationUser();
                }


            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.Claims.FirstOrDefault();
                var userId = User.FindFirstValue(ClaimTypes.Name);
                IQueryable<AplicationUser> query = _euroPlitkaDbContext.AplicationUser.Where(u => u.Id == claim.Value);
                aplicationUser = query.FirstOrDefault();
            }

            //access to the shopping cart(get access to the session,
            //load a snapshot from the session and extract the list of items in the cart based on this session)

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).ToList();
            }
            List<int> productCart = shoppingCartList.Select(x => x.ProductId).ToList();
            productuserViewModel = new ProductUserViewModel()
            {
                AplicationUser = aplicationUser
            };

            //обновление данных (кол-тва)(вытаскиваем колличество)
            foreach (var cartObj in shoppingCartList)
            {
                Product prodTemp = await _productRepository.FirstOrDefault(u => u.Id == cartObj.ProductId);
                prodTemp.TempSqFt = cartObj.Sqft;
                productuserViewModel.ProductList.Add(prodTemp);
            }
            return View(productuserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(IFormCollection collection, ProductUserViewModel productUserViewModel)
        {

            //get id user
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);
            OrderHeader orderHeader = new OrderHeader()
            {
                CreatedByUserId = claim.Value, //содержит id текущего пользователя
                FinalOrderTotal = productuserViewModel.ProductList.Sum(x => x.TempSqFt * x.Price), // общая сумма товвара(Linq)
                City = "CityTest",
                StreetAddress = "CityTestAddres",
                State = "UA",
                PostalCode = "0744",
                FullName = productuserViewModel.AplicationUser.FullName,
                Email = productuserViewModel.AplicationUser.Email,
                PhoneNumber = productuserViewModel.AplicationUser.PhoneNumber,
                OrderDate = DateTime.Now,
                countItem = productuserViewModel.ProductList.Count()

            };
            //add orderHeader in DB
            _orderHeaderRepository.Add(orderHeader);
            _orderHeaderRepository.Save();

            //each product from the ProductList must be added to Order_Detail
            foreach (var product in productUserViewModel.ProductList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderHeaderId = orderHeader.Id,
                    PricePerSqFt = product.Price,
                    Sqft = product.TempSqFt,
                    ProductId = product.Id
                };
                _orderDetailRepository.Add(orderDetail);
            }
            _orderDetailRepository.Save();
            return RedirectToAction(nameof(InquiryConfirmation), new { id = orderHeader.Id });
        }

        //View Confirm
        //if id has a valid value, then the method is approved for the situation with the placed order (in the case of the admin)
        public async Task<IActionResult> InquiryConfirmation(int id = 0)
        {
            OrderHeader orderHeader = await _orderHeaderRepository.FirstOrDefault(u => u.Id == id);
            //clearing the data of the current sessions. Because for the current session,
            //all the products that the client was interested in have already been included in the request
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);
            var basketUser = await _basketRepo.GetAll(u => u.CreatedByUserId == claim.Value);


            _basketRepo.RemoveRange(basketUser);


            HttpContext.Session.Clear();
            return View(orderHeader);
        }






        public async Task<IActionResult> Remove(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
               HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartsList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).ToList();
            }

            shoppingCartsList.Remove(shoppingCartsList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartsList); //установка сесси после удаления


            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);

            //Remove Basket
            var basetUser = await _basketRepo.FirstOrDefault(x => x.ProductId == id && x.CreatedByUserId == claim.Value);
            _basketRepo.Delete(basetUser);


            TempData[WebConstanta.Success] = "Product Remote to cart successfully";
            return RedirectToAction(nameof(Index));
        }



















        public async Task<IActionResult> RemoveAll(int id)
        {
            HttpContext.Session.Clear();
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);
            var basketUser = await _basketRepo.GetAll(u => u.CreatedByUserId == claim.Value);


            _basketRepo.RemoveRange(basketUser);

            TempData[WebConstanta.Success] = "All Product Remote to cart successfully";
            return RedirectToAction("Index", "Home");
        }

        //update Cart
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateCart(IEnumerable<Product> prodList)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier);
            //enumerate all objects Product from our enumeration
            foreach (Product prod in prodList)
            {
                shoppingCartsList.Add(new ShoppingCart
                {
                    ProductId = prod.Id,
                    Sqft = prod.TempSqFt
                });

                List<Basket> basketsList = new List<Basket>
                {
                    await _basketRepo.FirstOrDefault(x => x.ProductId == prod.Id) 
                };
               


                Basket findProd = await _basketRepo.FirstOrDefault(x => x.CreatedByUserId == claim.Value && x.ProductId == prod.Id);
                if (findProd != null)
                {
                    findProd.Sqft = prod.TempSqFt;
                }
                _basketRepo.Update(findProd);

            }
          




            //set value for current session
            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartsList);
            return RedirectToAction(nameof(Index));

        }

    }
}
