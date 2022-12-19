using EuroPlitka_DataAccess.Data;
using EuroPlitka_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Extensions
{


    public class EFStringLocalizerFactory : IStringLocalizerFactory
    {
        string _connectionString;
        public EFStringLocalizerFactory(string connection)
        {
            _connectionString = connection;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return CreateStringLocalizer();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return CreateStringLocalizer();
        }

        private IStringLocalizer CreateStringLocalizer()
        {
            EuroPlitkaDbContext _db = new EuroPlitkaDbContext(
                new DbContextOptionsBuilder<EuroPlitkaDbContext>()
                    .UseSqlServer(_connectionString)
                    .Options);
            // инициализация базы данных
            //if (!_db.Cultures.Any())
            //{
            //    _db.AddRange(
            //        new Culture
            //        {
            //            Name = "en",
            //            Resources = new List<Resource>()
            //            {
            //                new Resource { Param = "Header", Value = "Hello" },
            //                new Resource { Param = "Message", Value = "Welcome" }
            //            }
            //        },
            //        new Culture
            //        {
            //            Name = "uk",
            //            Resources = new List<Resource>()
            //            {
            //                new Resource { Param = "Header", Value = "Привет" },
            //                new Resource { Param = "Message", Value = "Добро пожаловать" }
            //            }
            //        }
                  
            //    );
            //    _db.SaveChanges();
            //}
            return new EFStringLocalizer(_db);
        }
    }


}
