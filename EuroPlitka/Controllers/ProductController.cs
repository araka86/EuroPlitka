using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;
using static System.Environment;


namespace EuroPlitka.Controllers
{
    public class ProductController : Controller
    {
        Bitmap image;

        private readonly IProductRepository _prodRepo;

        //dependency injection
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository prodRepo, IWebHostEnvironment webHostEnvironment)
        {
            _prodRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objtList = _prodRepo.GetAll(includeProperties: "Category,ProductType");
            return View(objtList);
        }

        //Get - Upsert(Views-->Index (create/update))
        public IActionResult Upsert(int? id)
        {

            ProdoctVM prodoctVM = new ProdoctVM()
            {
                Product = new Product(),
                CategorySelectList = _prodRepo.GetAllDropdownList(WebConstanta.CategoryName),
                ProductTypeSelectList = _prodRepo.GetAllDropdownList(WebConstanta.ProductTypeName)
            };
            if (id == null) //Check object
            {
                //this is for create
                return View(prodoctVM);
            }
            else
            {
                //update
                prodoctVM.Product = _prodRepo.Find(id.GetValueOrDefault());
                if (prodoctVM == null)
                {
                    return NotFound();
                }
                if (prodoctVM.Product.imagebyte !=null)
                {
                    string base64ImageRepresentation = Convert.ToBase64String(prodoctVM.Product.imagebyte);
                    string imgDataURL = string.Format("data:image/jpg;base64,{0}", base64ImageRepresentation);
                    ViewBag.Image = imgDataURL;
                }              
                return View(prodoctVM);
            }
        }
        public void CheckFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                var existFile = Directory.GetFiles(path);
                if (existFile.Length > 0)
                {
                    foreach (var item in existFile)
                    {
                        System.IO.File.Delete(item);
                    }
                }
            }
        }

        //Post - Upsert (Views-->Upsert(only UPDATE) )
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProdoctVM prodoctVM)
        {
            if (ModelState.IsValid)
            {
                string PublicPictureFolder = GetFolderPath(SpecialFolder.CommonPictures);
                string FullPathToDirectory = Path.Combine(PublicPictureFolder + WebConstanta.ImageFolder);
                CheckFolder(FullPathToDirectory);
                var files = HttpContext.Request.Form.Files; //get image
                if (prodoctVM.Product.Id == 0)
                {
                    //////////////////creating/////////////////////////
                    string fileName = Guid.NewGuid().ToString();       //greate random guid
                    string extension = Path.GetExtension(files[0].FileName); //get extension file which uploaded
                    string FullPath = Path.Combine(FullPathToDirectory, fileName + extension);
                    using (var filestream = new FileStream(FullPath, FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    byte[] contents = System.IO.File.ReadAllBytes(FullPath);  //crush to byte
                    prodoctVM.Product.imagebyte = contents;
                    _prodRepo.Add(prodoctVM.Product);
                    TempData[WebConstanta.Success] = "Prodoct Create successfully";
                }
                else
                {
                    ////////////////////////////////////updating//////////////////////////////////
                    var objFromDB = _prodRepo.FirstOrDefault(u => u.Id == prodoctVM.Product.Id, isTracking: false);
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();       //greate random guid
                        string extension = Path.GetExtension(files[0].FileName); //get extension file which uploaded
                        string FullPath = Path.Combine(FullPathToDirectory, fileName + extension);
                        using (var filestream = new FileStream(FullPath, FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        byte[] contents = System.IO.File.ReadAllBytes(FullPath);  //crush to byte
                        prodoctVM.Product.imagebyte = contents;
                    }
                    else
                    {
                        prodoctVM.Product.imagebyte = objFromDB.imagebyte;
                    }
                    TempData[WebConstanta.Success] = "Prodoct Update successfully";
                    _prodRepo.Update(prodoctVM.Product);
                }
                CheckFolder(FullPathToDirectory);
                _prodRepo.Save();
                return RedirectToAction("Index"); //return to Action
            }
            //if no valid
            prodoctVM.CategorySelectList = _prodRepo.GetAllDropdownList(WebConstanta.CategoryName);
            prodoctVM.ProductTypeSelectList = _prodRepo.GetAllDropdownList(WebConstanta.ProductTypeName);
            return View(prodoctVM);
        }





    }
}
