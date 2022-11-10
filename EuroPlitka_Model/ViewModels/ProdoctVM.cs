using Microsoft.AspNetCore.Mvc.Rendering;

namespace EuroPlitka_Model.ViewModels
{
    public class ProdoctVM
    {
        public Product? Product { get; set; }
      

       
        public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
        public IEnumerable<SelectListItem>? ProductTypeSelectList { get; set; }
    }
}
