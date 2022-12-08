using System.Diagnostics;

namespace EuroPlitka_Model.ViewModels
{
    public class CategoryMenuVM
    {



        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Category>? Categories { get; set; }

        public IEnumerable<ProductType>? ProductTypes { get; set; }

        public IEnumerable<Product> ProductsCat { get; set; }
        public Category CategoryProduct { get; set; }


        



        public ProductType? ProductType { get; set; }

       



        public IEnumerable<Product> ConditionProducts { get; set; }
   
        public int PageSize { get; set; }
        public int Page { get; set; }
        public bool AllPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCountAllCurrentCategory { get; set; }
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;







    }
}
