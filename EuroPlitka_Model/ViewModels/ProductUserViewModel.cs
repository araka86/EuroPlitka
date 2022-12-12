namespace EuroPlitka_Model.ViewModels
{
    public class ProductUserViewModel
    {

        public AplicationUser? AplicationUser { get; set; }
        public IList<Product>? ProductList { get; set; }

        public ProductUserViewModel()
        {
            ProductList = new List<Product>();

        }

        public string TypeOfDelivery { get; set; }
        public string TypeOfPayment { get; set; }
    }
}
