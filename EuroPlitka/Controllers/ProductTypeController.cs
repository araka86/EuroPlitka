using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductTypeController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductType> objtList = _productTypeRepository.GetAll();
            return View(objtList);
        }

        //Get - Greate
        public IActionResult Create() => View();

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductType obj)
        {
            if (ModelState.IsValid)
            {
                _productTypeRepository.Add(obj);
                _productTypeRepository.Save();
                TempData[WebConstanta.Success] = "AplicationType create successfully";
                return Redirect("Index");
            }
            TempData[WebConstanta.Error] = "AplicationType create Error";
            return View(obj);
        }



        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _productTypeRepository.Find(id.GetValueOrDefault());

            if (obj == null)
                return NotFound();


            return View(obj);
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductType obj)
        {

            if (ModelState.IsValid)
            {
                _productTypeRepository.Update(obj);
                _productTypeRepository.Save();
                TempData[WebConstanta.Success] = "AplicationType Update successfully";
                return RedirectToAction("Index");
            }
            TempData[WebConstanta.Error] = "AplicationType Update Error";
            return View(obj);
        }





        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var obj = _productTypeRepository.Find(id.GetValueOrDefault());

            if (obj == null)
                return NotFound();


            return View(obj);
        }


        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _productTypeRepository.Find(id.GetValueOrDefault());
            if (obj == null)
                return NotFound();


            _productTypeRepository.Remove(obj);
            _productTypeRepository.Save();
            TempData[WebConstanta.Success] = "AplicationType Delete successfully";
            return RedirectToAction("Index");
        }





    }
}
