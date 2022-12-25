using static System.Environment;

namespace EuroPlitka_Services
{
    public  static class WebConstanta
    {
        //For roles
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        //For porcess message
        public const string Success = "Success";
        public const string Error = "Error";


        public const string CategoryName = "Category";
        public const string ProductTypeName = "ProductTypeName";
        public const string ViewName = "VIew";
        public const string Pagefille = "Pagefille";


        //For Path to image
        public const string ImageFolder = @"\images";

        //For session
        public const string SessionCart = "ShoppingCartSession";
        //ключ для храниния значения ID запроса текущего сеанса
        public const string SessionInquiryId = "InquirySession";


        public const string connectToDb = "Server=DESKTOP-ESCI621; Database=EuroPlitka2; Trusted_Connection=True; MultipleActiveResultSets=True; TrustServerCertificate=True";

        //forTest
        public const string TestUserName = "TestUser";
        public const string TestIdUser = "TestUser";




        //remove duplicate for category product type
        public static T[] RemoteDuplicates<T>(T[] array)
        {
            HashSet<T> set = new HashSet<T>(array);
            T[] result = new T[set.Count];
            set.CopyTo(result);
            return result;
        }
     
       

    }
}
