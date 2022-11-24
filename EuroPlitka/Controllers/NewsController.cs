using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
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
           
            return View(news.ToList());
        }

        public async Task<IActionResult> Upsert(List<News> News)
        {



            return View();
        }

        public void OnPost(IEnumerable<News> News)
        {
            // process the contacts
        }



    }
}
