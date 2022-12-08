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
        public async Task<IActionResult> Index(ProdoctVM prodoctVM, bool reset = false)
        {

            ProdoctVM prodocts = new ProdoctVM()
            {
                Products = await _prodRepo.GetAll(includeProperties: "Category,ProductType"),
                CategorySelectList = await _prodRepo.GetAllDropdownList(WebConstanta.CategoryName),
                ProductTypeSelectList = await _prodRepo.GetAllDropdownList(WebConstanta.ProductTypeName)
            };
            if (reset == false)
            {
                if (prodoctVM.NameProduct != null || prodoctVM.CategoryListFilter != null || prodoctVM.ProductListFilter != null)
                {

                    if (!string.IsNullOrEmpty(prodoctVM.NameProduct))
                    {
                        prodocts.Products = prodocts.Products.Where(x => x.Name.ToLower().Contains(prodoctVM.NameProduct.ToLower()));
                    }
                    if (!string.IsNullOrEmpty(prodoctVM.CategoryListFilter) && prodoctVM.CategoryListFilter != "--Select Category--")
                    {
                        prodocts.Products = prodocts.Products.Where(x => x.CategoryId.ToString() == prodoctVM.CategoryListFilter);
                    }
                    if (!string.IsNullOrEmpty(prodoctVM.ProductListFilter) && prodoctVM.ProductListFilter != "--Select ProductType--")
                    {

                        prodocts.Products = prodocts.Products.Where(x => x.ProductTypeId.ToString() == prodoctVM.ProductListFilter);
                    }
                    return View(prodocts);
                }

            }


          
     
            ModelState.Clear();
            return View(prodocts);
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
                //if (prodoctVM.Product.Description.Contains("<img"))
                //{
                //    //logical for summrnote img
                //}

                var files = HttpContext.Request.Form.Files; //get image
                if (prodoctVM.Product.Id == 0)//create
                {
                    byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
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



            _prodRepo.Delete(obj);
            TempData[WebConstanta.Success] = "Prodoct Delete successfully";
            return RedirectToAction("Index");
        }

    }
}
