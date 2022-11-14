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
        private readonly EuroPlitkaDbContext _euroPlitkaDbContext;

        [BindProperty] 
        public ProductUserViewModel productuserViewModel { get; set; }

        public CartController(IProductRepository productRepository, 
            IOrderDetailRepository orderDetailRepository,
            IOrderHeaderRepository orderHeaderRepository,
            EuroPlitkaDbContext euroPlitkaDbContext
            )
        {
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderHeaderRepository = orderHeaderRepository;
            _euroPlitkaDbContext = euroPlitkaDbContext;
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
                Product prodtemp = productsListTemp.FirstOrDefault(u => u.Id == cartObj.ProductId); //получаем обьект  prodtemp

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


            //перебор всех обьектов Product из нaшего перечисления
            foreach (Product prod in prodList)
            {
                shoppingCartsList.Add(new ShoppingCart
                {
                    ProductId = prod.Id,
                    Sqft = prod.TempSqFt
                });
            }
            //задать значение для текущей сессии

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

                if (HttpContext.Session.Get<int>(WebConstanta.SessionInquiryId) != 0) //эсли не равно 0 - то значит обрабатывакется какой-то запрос
                {
                    //cart has been loaded using an inquiry


                    OrderHeader orderHeader =  await _orderHeaderRepository.FirstOrDefault(u => u.Id == HttpContext.Session.Get<int>(WebConstanta.SessionInquiryId));

                    //заполнения корзины на основании текущего запроса
                    aplicationUser = new AplicationUser()
                    {
                        Email = orderHeader.Email,
                        FullName = orderHeader.FullName,
                        PhoneNumber = orderHeader.PhoneNumber

                    };
                }
                else
                {
                    //админ размещает заказ клиента который просто пришел в магазин не используя сайт
                    aplicationUser = new AplicationUser();
                }

    
            }
            else
            {

                if (HttpContext.Session.Get<int>(WebConstanta.SessionInquiryId) != 0) //эсли не равно 0 - то значит обрабатывакется какой-то запрос
                {

                }

                // данные юзера (name, mail,tel)
                //получение  id юзера (обьект будет получень эсли юзер вошел в систему)
                /////////Variant1//////////////
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.Claims.FirstOrDefault();
                var userId = User.FindFirstValue(ClaimTypes.Name);

                IQueryable<AplicationUser> query = _euroPlitkaDbContext.AplicationUser.Where(u => u.Id == claim.Value);

                aplicationUser = query.FirstOrDefault();

         
            }



            //доступ к корзине покупок(получить доступ к сессии,
            //загрузить спимок из сесии и извлеч список товаров в корзине на основе єтой сессии)

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
        public async Task<IActionResult> SummaryPost(IFormCollection collection, ProductUserViewModel productUserViewModel) //  а так же будет доступ через привязаное свойство  productuserViewModel
        {

            //BrainTree - при получении данных из формы, так же получаем значение токeна nonce SummaryPost


            //ПОЛУЧЕНИЕ id  пользователя
            var claimsIndentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIndentity.FindFirst(ClaimTypes.NameIdentifier); //искомое значение

           
               

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
                //добавление orderHeader в БД
                _orderHeaderRepository.Add(orderHeader);
                _orderHeaderRepository.Save();


                //каждый товар из списка ProductList нужно добавлять в OrderDetail
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

          




            //if (User.IsInRole(WebConstanta.CustomerRole))
            //{

            //    //путь к шаблону
            //    var PathToTemplate = _webHostEnviroment.WebRootPath
            //        + Path.DirectorySeparatorChar.ToString()
            //        + "templates" + Path.DirectorySeparatorChar.ToString()
            //        + "Inquiry.html";

            //    var subject = "New Inquiry";
            //    string HtmlBody = "";

            //    using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            //    {
            //        HtmlBody = sr.ReadToEnd();
            //    }

            //    //Name:     {0}
            //    //Email:    {1}
            //    //Phone:    {2}
            //    //Products: {3}

            //    StringBuilder productListSB = new StringBuilder();

            //    foreach (var product in productUserViewModel.ProductList)
            //    {
            //        productListSB.Append($"- Name {product.Name}<span style='font-size:14px;' >(ID: {product.Id})</span><br />");
            //    }

            //    string messageBody = string.Format(HtmlBody,
            //        productUserViewModel.AplicationUser.FullName,
            //        productUserViewModel.AplicationUser.Email,
            //    productUserViewModel.AplicationUser.PhoneNumber,
            //        productListSB.ToString());


            //    await _emailSender.SendEmailAsync(WebConstanta.EmailAdmin, subject, messageBody);

            //    return RedirectToAction(nameof(InquiryConfirmation)); // перенаправление в метод
            //}

            return RedirectToAction(nameof(InquiryConfirmation), new { id = orderHeader.Id });



        }

        //View Confirm
        //эсли  id имеет валидное значение - то значит  метод визивается для ситуации с размещенным заказом (в случае с админом)
        public async Task<IActionResult> InquiryConfirmation(int id = 0)
        {
            OrderHeader orderHeader = await _orderHeaderRepository.FirstOrDefault(u => u.Id == id);

            // очистка данных текущей сессий. Т.к для текущей сесии, все товары которые интересовали клиента уже попали в запрос
            HttpContext.Session.Clear();

            return View(orderHeader);

        }

        public IActionResult RemoveAll(int id)
        {

            HttpContext.Session.Clear();

            TempData[WebConstanta.Success] = "All Product Remote to cart successfully";
            return RedirectToAction("Index", "Home");
        }

        //update Cart
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateCart(IEnumerable<Product> prodList)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();



            //перебор всех обьектов Product из нaшего перечисления
            foreach (Product prod in prodList)
            {
                shoppingCartsList.Add(new ShoppingCart
                {
                    ProductId = prod.Id,
                    Sqft = prod.TempSqFt
                });
            }
            //задать значение для текущей сессии


            HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartsList);

            return RedirectToAction(nameof(Index));

        }




















    }
}
