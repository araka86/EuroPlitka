using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<AplicationUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index() => View(_roleManager.Roles.ToList());    
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData[WebConstanta.Success] = "Role Create successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            TempData[WebConstanta.Error] = "Role create error!!!!";
            return View(name);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                TempData[WebConstanta.Success] = "Role delete successfully";
                return RedirectToAction("Index");
            }
            TempData[WebConstanta.Error] = "Role delete error!!!!!";
            return RedirectToAction("Index");
        }

        //change role for user
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            AplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("EditProfile", "User", new { user.Id });
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole identityRole = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();
            return View(identityRole);
        }

        [HttpPost("/Roles/EditRole/{id}")]
        [ActionName("EditRole")]
        public async Task<IActionResult> EditRolePost(IdentityRole identityRole)
        {

            var role = await _roleManager.FindByIdAsync(identityRole.Id);
            if (role != null)
            {
                role.NormalizedName = identityRole.Name;
                role.Id = identityRole.Id;
                role.Name = identityRole.Name;
                var idResult = await _roleManager.UpdateAsync(role);
                if (!idResult.Succeeded)
                {
                    TempData[WebConstanta.Error] = "Role update Error";
                    return RedirectToAction("Index", "Roles");
                }     
            }
            TempData[WebConstanta.Success] = "Role Update successfully";
            return RedirectToAction("Index", "Roles");

        }
    }
}
