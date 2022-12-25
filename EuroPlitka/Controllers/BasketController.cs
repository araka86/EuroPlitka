using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class BasketController : Controller
    {
       

        public BasketController()
        {

        }





        public IActionResult Index()
        {
            return View();
        }




    }
}
