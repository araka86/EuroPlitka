using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AplicationUser> _userManager;
        private readonly SignInManager<AplicationUser> _signInManager;
        private readonly IBasketRepo _basketRepo;
        public AccountController(UserManager<AplicationUser> userManager, SignInManager<AplicationUser> signInManager,IBasketRepo basketRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _basketRepo = basketRepo;
        }

        [BindProperty]
        public LoginRegistrViewModel Input { get; set; }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> RegisterMenu()
        {
            if (ModelState.IsValid)
            {
                string returnUrl = Url.Content("~/");
                var user = new AplicationUser
                {
                    UserName = Input.FullName,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    FullName = Input.FullName,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded) //When the administrator registers, add role admin
                {
                    if (User.IsInRole(WebConstanta.AdminRole))
                    {
                        await _userManager.AddToRoleAsync(user, WebConstanta.AdminRole);
                        TempData[WebConstanta.Success] = "Admin Create successfully";
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, WebConstanta.CustomerRole);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public   IActionResult  Login(string returnUrl = null)
        {
            return  View(new LoginRegistrViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login()
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(Input.ReturnUrl) && Url.IsLocalUrl(Input.ReturnUrl))
                        {
                            return Redirect(Input.ReturnUrl);
                        }
                        else
                        {

                            var getbasketUser = _basketRepo.GetAll(x => x.CreatedByUserId == user.Id).Result;

                            if (getbasketUser!=null)
                            {
                                List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

                                if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart) != null &&
                                   HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstanta.SessionCart).Any())
                                {
                                    shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstanta.SessionCart);
                                }
                                foreach (var item in getbasketUser)
                                {
                                    shoppingCartList.Add(new ShoppingCart { ProductId = (int)item.ProductId, Sqft = item.Sqft });
                                }
                                HttpContext.Session.Set(WebConstanta.SessionCart, shoppingCartList);
                            }

                         








                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect login and/or password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "A user with this address is not registered");
                }
            }

            return View(Input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
