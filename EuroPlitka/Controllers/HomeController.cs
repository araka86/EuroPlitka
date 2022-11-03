using EuroPlitka.Models;
using EuroPlitka_DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EuroPlitka.Controllers
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly EuroPlitkaDbContext _applicationDbContext;

        public CategoryViewComponent(EuroPlitkaDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _applicationDbContext.Categorys.ToListAsync();
            return View(result);
        }

    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}