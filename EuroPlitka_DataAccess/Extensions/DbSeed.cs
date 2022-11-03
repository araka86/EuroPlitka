using EuroPlitka_DataAccess.Data;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Extensions
{


    public class DbSeed : IDbSeed
    {


        private readonly EuroPlitkaDbContext _db;
        private readonly UserManager<AplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
     

        public DbSeed(EuroPlitkaDbContext db, UserManager<AplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public void Initialize()
        {

            // (create role and user (initializing)) .GetAwaiter().GetResult() - Instead of await
            if (!_roleManager.RoleExistsAsync(WebConstanta.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebConstanta.AdminRole)).GetAwaiter().GetResult(); //роль админа
                _roleManager.CreateAsync(new IdentityRole(WebConstanta.CustomerRole)).GetAwaiter().GetResult(); //роль юзера
            }
            _userManager.CreateAsync(new AplicationUser
            {
                UserName = "Admin",
                Email = "test@admin.com",
                EmailConfirmed = true,
                FullName = "Admin",
                PhoneNumber = "0931111111"
            }, "123!@#QWEqwe").GetAwaiter().GetResult();

            AplicationUser user = _db.AplicationUser.First(u => u.Email == "test@admin.com");
            _userManager.AddToRoleAsync(user, WebConstanta.AdminRole).GetAwaiter().GetResult();
         
        }

       


        public  void InitializeData(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<EuroPlitkaDbContext>();
                context.Database.EnsureCreated();
                if (!context.Categorys.Any())
                {
                    context.Categorys.AddRange(new List<Category>()
                    {
                        new Category{Name ="Category 1", DisplayOrder= 1},
                        new Category{Name ="Category 2", DisplayOrder= 2},
                        new Category{Name ="Category 3", DisplayOrder= 3},
                    });
                }
                context.SaveChanges();
                if (!context.ProductTypes.Any())
                {
                    context.ProductTypes.AddRange(new List<ProductType>()
                    {
                        new ProductType{Name = "qwe"},
                        new ProductType{Name = "qwe"},
                        new ProductType{Name = "qwe"},

                    });
                }
                context.SaveChanges();


            }


        }

       
    }
}
