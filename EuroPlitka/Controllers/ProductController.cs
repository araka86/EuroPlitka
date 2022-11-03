using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;
using System.IO;
using System.Net.Mail;


using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NuGet.Packaging.Signing;
using System;


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

          
            //byte[] contents = System.IO.File.ReadAllBytes("D:\\МНТУ\\Final Сессия\\Бакалаврська робота\\EuroPlitka\\EuroPlitkakover.jpg");

            //string base64ImageRepresentation = Convert.ToBase64String(contents);

            //var base64 = $"data:image/png;base64,{Convert.ToBase64String(contents)}";
            //string imgDataURL = string.Format("data:image/jpg;base64,{0}", base64ImageRepresentation);

            //var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64ImageRepresentation)));

       
            //ViewBag.Image = base64;




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

                string base64ImageRepresentation = Convert.ToBase64String(prodoctVM.Product.imagebyte);
                string imgDataURL = string.Format("data:image/jpg;base64,{0}", base64ImageRepresentation);
                ViewBag.Image = imgDataURL;
                return View(prodoctVM);
            }

        }

/// /////////////////////////////////////////////////

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return Image.FromStream(ms, true);
            }
        }

        /// /////////////////////////////////////////////////




        //Post - Upsert (Views-->Upsert(only UPDATE) )
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProdoctVM prodoctVM)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files; //get image
                string webRootPath = _webHostEnvironment.WebRootPath; //get path to wwwroot

               
                if (prodoctVM.Product.Id == 0)
                {

                    //creating
                    string upload = webRootPath + WebConstanta.ImagePath; //get path from image
                    string fileName = Guid.NewGuid().ToString();       //greate random guid
                    string extension = Path.GetExtension(files[0].FileName); //get extension file which uploaded
                    string FullPath = Path.Combine(upload, fileName + extension);

                    using (var filestream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    byte[] contents = System.IO.File.ReadAllBytes(FullPath);  //crush to byte

                    prodoctVM.Product.imagebyte = contents;
                    _prodRepo.Add(prodoctVM.Product);


                }
                else
                {

                }

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
