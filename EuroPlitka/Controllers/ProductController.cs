using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;


namespace EuroPlitka.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductRepository _prodRepo;

        public ProductController(IProductRepository prodRepo)
        {
            _prodRepo = prodRepo;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objtList = await _prodRepo.GetAll(includeProperties: "Category,ProductType");
            return View(objtList);
        }

        //Get - Upsert(Views-->Index (create/update))
        public async Task<IActionResult> Upsert(int? id)
        {

            ProdoctVM prodoctVM = new ProdoctVM()
            {
                Product = new Product(),
                CategorySelectList = await _prodRepo.GetAllDropdownList(WebConstanta.CategoryName),
                ProductTypeSelectList = await _prodRepo.GetAllDropdownList(WebConstanta.ProductTypeName)
            };
            if (id == null) //Check object
            {
                //this is for create
                return View(prodoctVM);
            }
            else
            {
                //update
                prodoctVM.Product = await _prodRepo.Find(id.GetValueOrDefault());
                if (prodoctVM == null)
                    return NotFound();
                
                    
                return View(prodoctVM);
            }
        }
     

        //Post - Upsert (Views-->Upsert(only UPDATE) )
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProdoctVM prodoctVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files; //get image
                if (prodoctVM.Product.Id == 0)//create
                {
                    byte[] resultToByte = await  PhotoService.FileToByte(files); //get byte
                    prodoctVM.Product.imagebyte = resultToByte;
                     _prodRepo.Add(prodoctVM.Product);
                    TempData[WebConstanta.Success] = "Prodoct Create successfully";
                }
                else  ////////////////////////////////////updating if need to change images//////////////////////////////////
                {
                   
                    var objFromDB = await _prodRepo.FirstOrDefault(u => u.Id == prodoctVM.Product.Id, isTracking: false);
                    if (files.Count > 0)
                    {
                        byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                        prodoctVM.Product.imagebyte = resultToByte;
                    }
                    else ////////////////////////////////////updating if will not need to change images//////////////////////////////////
                    {
                        prodoctVM.Product.imagebyte = objFromDB.imagebyte;
                    }
                    TempData[WebConstanta.Success] = "Prodoct Update successfully";
                    _prodRepo.Update(prodoctVM.Product);
                }
           
                _prodRepo.Save();
                return RedirectToAction("Index"); //return to Action
            }
            //if no valid
            prodoctVM.CategorySelectList = await _prodRepo.GetAllDropdownList(WebConstanta.CategoryName);
            prodoctVM.ProductTypeSelectList = await _prodRepo.GetAllDropdownList(WebConstanta.ProductTypeName);
            return View(prodoctVM);
        }




        //Get - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Product product = await _prodRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Category,ProductType");


            if (product == null)
                return NotFound();



            return View(product);
        }


        //Post - Delete
        [HttpPost, ActionName("Delete")] //from  Product/Delete.cshtml
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = await _prodRepo.Find(id.GetValueOrDefault());
            if (obj == null)
                return NotFound();



            _prodRepo.Remove(obj);
            _prodRepo.Save();
            TempData[WebConstanta.Success] = "Prodoct Delete successfully";
            return RedirectToAction("Index");
        }

    }
}
