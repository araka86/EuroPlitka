using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
