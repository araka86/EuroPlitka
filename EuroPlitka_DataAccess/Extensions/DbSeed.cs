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
                        new Category{Name ="Для ванной", DisplayOrder= 1},
                        new Category{Name ="Для підлоги", DisplayOrder= 2},
                        new Category{Name ="Для стін", DisplayOrder= 3},
                        new Category{Name ="Для кухні", DisplayOrder= 4},
                        new Category{Name ="Вулична", DisplayOrder= 5}
                    
                     
                    });
                }
                context.SaveChanges();
                if (!context.ProductTypes.Any())
                {
                    context.ProductTypes.AddRange(new List<ProductType>()
                    {
                        new ProductType{Name = "На стелю"},
                        new ProductType{Name = "На підлогу"},
                        new ProductType{Name = "На стіну"},
                        new ProductType{Name = "На кухню"},
                        new ProductType{Name = "В коридор"},
                        new ProductType{Name = "На балкон"},
                        new ProductType{Name = "На сходи"},
                        new ProductType{Name = "В туалет"},
                        new ProductType{Name = "Вулична"},
                        new ProductType{Name = "В баню"},

                    });
                }
                context.SaveChanges();
                if(!context.Product.Any())
                {
                    context.Product.AddRange(new List<Product>() 
                    {
                       new Product
                       {
                           Name="Product1",
                           Price=200,
                           shortDesc = "shortDec",
                           Description="decriotion"
                       }
                    });
                }
                context.SaveChanges();


            }


        }

       
    }
}
