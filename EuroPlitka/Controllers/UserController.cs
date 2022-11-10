using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AplicationUser> _userManager;
        private readonly EuroPlitkaDbContext _db;
      


        public UserController(UserManager<AplicationUser> userManager, EuroPlitkaDbContext db)
        {
            _userManager = userManager;
            _db = db;

          

        }
        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> EditProfile(AplicationUser editVM, byte[]imgbyte=null)
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
                var photoResult = await PhotoService.FileToByte(getFile, _db); //get byte
                user.imgUserAva = photoResult;

               
            }
            user.FullName = editVM.FullName;
            user.City = editVM.City;
            user.StreetAddress = editVM.StreetAddress;
            user.Description = editVM.Description;
            await _userManager.UpdateAsync(user);



            TempData[WebConstanta.Success] = "User Update successfully";



        

            return RedirectToAction("Detail", "User", new { user.Id });
        }




        //[HttpGet]
        //public async Task<IActionResult> Detail(string id)
        //{

        //    var user = await _userManager.GetUserAsync(id);
        //    if (user == null)
        //    {
        //        return RedirectToAction("Index", "Users");
        //    }

        //    var userDetailViewModel = new AplicationUser()
        //    {
        //        Id = user.Id,
        //        FullName = user.FullName,
        //        StreetAddress = user.StreetAddress,
        //        City = user.City,
        //        Description = user.Description,
        //        Email = user.Email,
        //        imgUserAva = user.imgUserAva
               
                
               
              
        //    };
        //    return View(userDetailViewModel);
        //}







    }
}
