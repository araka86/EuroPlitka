using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_DataAccess.Extensions
{
    public interface IDbSeed
    {
        public void Initialize();
       public  void InitializeData(IApplicationBuilder applicationBuilder);

    }
}
