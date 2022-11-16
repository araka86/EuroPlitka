﻿using EuroPlitka_Model;
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
        public AccountController(UserManager<AplicationUser> userManager, SignInManager<AplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
