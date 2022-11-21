using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_DataAccess.Repository.IReposotory;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IProductRepository _productRepository;

        public CategoryController(ICategoryRepository catRepo, IProductRepository productRepository)
        {
            _catRepo = catRepo;
            _productRepository = productRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objtList = await _catRepo.GetAll();
            return View(objtList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _catRepo.Add(obj);             
                TempData[WebConstanta.Success] = "Catogory created successfully";
                return Redirect("Index");
            }
            TempData[WebConstanta.Error] = "Error while creating category ";
            return View(obj);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();


            var obj = await _catRepo.Find(id.GetValueOrDefault());
            if (obj == null)
                return NotFound();

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _catRepo.Update(obj);
               
                TempData[WebConstanta.Success] = "Catogory update successfully";
                return RedirectToAction("Index");
            }
            TempData[WebConstanta.Error] = "Catogory update Error";
            return View(obj);
        }

        //Get - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = await _catRepo.Find(id.GetValueOrDefault());

            if (obj == null)
                return NotFound();


            return View(obj);
        }
        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = await _catRepo.Find(id.GetValueOrDefault());


            if (obj == null)
                return NotFound();



            var findReference = await _productRepository.FirstOrDefault(x => x.CategoryId == obj.Id);
            if (findReference != null)
            {
                TempData[WebConstanta.Error] = "Catogory Can't be  Delete, because he has a product!!!!";
                return RedirectToAction("Delete",new { obj.Id });
            }
                





            _catRepo.Delete(obj);
            TempData[WebConstanta.Success] = "Catogory Delete successfully";
            return RedirectToAction("Index");
        }
    }
}
