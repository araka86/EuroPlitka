using Microsoft.AspNetCore.Mvc.Rendering;

namespace EuroPlitka_Model.ViewModels
{
    public class ProdoctVM
    {
        public Product? Product { get; set; }
        public IEnumerable<Product>? Products { get; set; }

        public string? NameProduct { get; set; }
        public string? CategoryListFilter { get; set; }
        public string? ProductListFilter { get; set; }

    
        public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
        public IEnumerable<SelectListItem>? ProductTypeSelectList { get; set; }
    }
}
