using EuroPlitka_DataAccess.Data;
using EuroPlitka_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka.Test.Data
{
    public static class InitializeStartData
    {


        public static async Task<EuroPlitkaDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<EuroPlitkaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new EuroPlitkaDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Product.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Product.Add(
                      new Product()
                      {
                          Name = i.ToString(),
                          shortDesc = "shDesc",
                          Description = "desc",
                          Price = 100 + i,
                          Category = new Category()
                          {
                              NameUa = $"UaName {i}",
                              NameEng = $"EngName {i}"
                          },
                          ProductType = new ProductType()
                          {
                              Name = $"Type {i}"
                          }
                      });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }





    }
}
