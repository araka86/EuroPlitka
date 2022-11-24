using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepositoriy _newsRepositoriy;

        [BindProperty]
        public IEnumerable<News> news { get; set; }


        public NewsController(INewsRepositoriy newsRepositoriy)
        {
            _newsRepositoriy = newsRepositoriy;
        }

        public async Task<IActionResult> Index()
        {
            news = await _newsRepositoriy.GetAll();



            return View(news.OrderByDescending(x => x.DateTime).ToList());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }


        public async Task<IActionResult> UpateVision(IEnumerable<News> News)
        {

            if (_newsRepositoriy.UpdateRange(News))
            {
                TempData[WebConstanta.Success] = "News Visible successfully update";
                return RedirectToAction("Index");
            }
            TempData[WebConstanta.Error] = "News Visible Error!!!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {



            return View();
        }


    }
}
