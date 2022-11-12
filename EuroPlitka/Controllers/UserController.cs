﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.FileManager;
using System.Data;
using System.Security.Claims;

namespace EuroPlitka.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

       
        private readonly IUserRepository _userRepository;


        public UserController(UserManager<AplicationUser> userManager, IUserRepository userRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
           
            _userRepository = userRepository;
            _roleManager = roleManager;
        }



        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {

            IEnumerable<AplicationUser> users = await _userRepository.GetAll();

            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;

            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            var rolesss = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            var rolesssdf = _roleManager.Roles.ToList();



            foreach (var item in users)
            {
                var roless = await _userManager.GetRolesAsync(item);

            }






            return View(users);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return View("Error");


            var editMV = new AplicationUser()
            {
                FullName = user.FullName,
                City = user.City,
                Description = user.Description,
                imgUserAva = user.imgUserAva,
                StreetAddress = user.StreetAddress

            };
            return View(editMV);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(AplicationUser editVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editVM);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return View("Error");









            //if (editVM.Image != null)
            //{
            //    var getFile2 = HttpContext.Request.Form.Files;
            //    var photoResult = await PhotoService.FileToByte(getFile2, _db); //get byte
            //    user.imgUserAva = photoResult;// temp
            //    return View(user);

            //}



            var getFile = HttpContext.Request.Form.Files;


            if (getFile.Count() > 0)
            {
                var photoResult = await PhotoService.FileToByte(getFile); //get byte
                user.imgUserAva = photoResult;


            }
            user.FullName = editVM.FullName;
            user.City = editVM.City;
            user.StreetAddress = editVM.StreetAddress;
            user.Description = editVM.Description;
            user.PhoneNumber = editVM.PhoneNumber;

            await _userManager.UpdateAsync(user);



            TempData[WebConstanta.Success] = "User Update successfully";





            return RedirectToAction("Detail", "User", new { user.Id });
        }




        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {

            var user = await _userRepository.GetUserById(id);


            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var userDetailViewModel = new AplicationUser()
            {
                Id = user.Id,
                FullName = user.FullName,
                StreetAddress = user.StreetAddress,
                City = user.City,
                Description = user.Description,
                Email = user.Email,
                imgUserAva = user.imgUserAva

            };
            return View(userDetailViewModel);
        }







    }
}
