using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_Model.ViewModels
{
    public class NewsProducstHomeVM
    {
      public  IEnumerable<News> News { get; set; }
      public  IEnumerable<Product> Products { get; set; }
    }
}
