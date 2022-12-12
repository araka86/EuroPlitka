using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace EuroPlitka.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOrderHeaderRepository _orderHRepo;
        private readonly IOrderDetailRepository _orderDRepo;
        private readonly IUserRepository _userRepository;

        public UserController(UserManager<AplicationUser> userManager, IUserRepository userRepository, 
            IOrderHeaderRepository orderHeaderRepository, IOrderDetailRepository orderDetailRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _orderHRepo = orderHeaderRepository;
            _orderDRepo = orderDetailRepository;
            _roleManager = roleManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {

            IEnumerable<AplicationUser> users = await _userRepository.GetAll();
            return View(users);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile(string? id)
        {
            var user = new UsersVM();
            if (id == null)
            {
                user.aplicationUser = await _userManager.GetUserAsync(User); //singin admin or user
                if (user.aplicationUser == null) return View("Error");
            }
            else
            {
                //user = await _userRepository.FirstOrDefault(x => x.Id == id);
                user.aplicationUser = await _userManager.FindByIdAsync(id);
                if (user.aplicationUser == null) return View("Error");
            }
            var userRoles = await _userManager.GetRolesAsync(user.aplicationUser);
            var allRoles = _roleManager.Roles.ToList();
            user.ChangeRoles = new ChangeRoleViewModel()
            {
                UserId = user.aplicationUser.Id,
                UserEmail = user.aplicationUser.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            user.EditUserVM.Id = user.aplicationUser.Id;
            user.EditUserVM.FullName = user.aplicationUser.FullName;
            user.EditUserVM.PhoneNumber = user.aplicationUser.PhoneNumber;
            user.EditUserVM.City = user.aplicationUser.City;
            user.EditUserVM.StreetAddress = user.aplicationUser.StreetAddress;
            user.EditUserVM.imgUserAva = user.aplicationUser.imgUserAva;

            return View(user);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(UsersVM editVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editVM);
            }
            var usr = await _userManager.FindByIdAsync(editVM.EditUserVM.Id); //get exsist record
            if (usr == null)
                return View("Error");

            var getFile = HttpContext.Request.Form.Files;
            if (getFile.Count() > 0)
            {
                var photoResult = await PhotoService.FileToByte(getFile); //get byte
                usr.imgUserAva = photoResult;
            }
            usr.FullName = editVM.EditUserVM.FullName;
            usr.City = editVM.EditUserVM.City;
            usr.StreetAddress = editVM.EditUserVM.StreetAddress;
            usr.Description = editVM.EditUserVM.Description;
            usr.PhoneNumber = editVM.EditUserVM.PhoneNumber;
            await _userManager.UpdateAsync(usr);
            TempData[WebConstanta.Success] = "User Update successfully";
            return RedirectToAction("Detail", "User", new { usr.Id });
        }
   
        [HttpGet]
        public async Task<IActionResult> DetailModal(string id)
        {
            var user = await _userRepository.FirstOrDefault(x => x.Id == id);
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
            return PartialView(userDetailViewModel);
        }



        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.FirstOrDefault(x => x.Id == id);
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


        [HttpGet("users/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var findUser = _userRepository.FirstOrDefault(x => x.Id == id);
            if (findUser == null)
                return NotFound();



            var delUser = new AplicationUser()
            {
                FullName = findUser.Result.FullName,
                City = findUser.Result.City,
                Description = findUser.Result.Description,
                imgUserAva = findUser.Result.imgUserAva,
                StreetAddress = findUser.Result.StreetAddress
            };
            return View(delUser);
        }
        [HttpPost("users/{Id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string? id)
        {
            var findUser = _userRepository.FirstOrDefault(x => x.Id == id);
            if (findUser == null)
                return NotFound();

            _userRepository.Delete(findUser.Result);
        
            TempData[WebConstanta.Success] = "User Delete successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AplicationUser changePassUser)
        {

            var user = await _userManager.GetUserAsync(User);
            var finduser = await _userManager.FindByIdAsync(changePassUser.Id);
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(finduser);
            var reset = await _userManager.ResetPasswordAsync(finduser, resetToken, changePassUser.Password);
            TempData[WebConstanta.Success] = "Password change successfully";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> OrdersUser()
        {
            var user = await _userManager.GetUserAsync(User);
            OrderListVm orderListVm = new OrderListVm()
            {
                OrderHeaderList = await _orderHRepo.GetAll(x => x.CreatedByUserId == user.Id),
            };
            return View(orderListVm);
        }

        public async Task<IActionResult> DetailsOrder(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = await _orderHRepo.FirstOrDefault(u => u.Id == id),
                OrderDetails = await _orderDRepo.GetAll(o => o.OrderHeaderId == id, includeProperties: "Product")
            };
            return View(orderVM);
        }
    }
}
