using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EuroPlitka.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepositoriy _newsRepositoriy;
        private UserManager<AplicationUser> _userManager;

        [BindProperty]
        public IEnumerable<News> newses { get; set; }
        [BindProperty]
        public News news { get; set; }


        public NewsController(INewsRepositoriy newsRepositoriy, UserManager<AplicationUser> userManager)
        {
            _newsRepositoriy = newsRepositoriy;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            newses = await _newsRepositoriy.GetAll();
            foreach (var item in newses)
            {
                if (item.CreatedByUserId != null)
                    item.CreatedBy = await _userManager.FindByIdAsync(item.CreatedByUserId);

            }



            return View(newses.OrderByDescending(x => x.DateTime).ToList());
        }










        public async Task<IActionResult> Upsert(int? id)
        {

            var news = new News();


            if (id == null)
            {
                return View(news);
            }
            else
            {
                news = await _newsRepositoriy.FirstOrDefault(x => x.Id == id);
                if (news == null)
                    return NotFound();

                return View(news);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert() //bind property
        {



            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files; //get image
                if (HttpContext.Request.Form.Files.Count() > 0)//if we add imageM OR imageSummernote
                {

                    if (HttpContext.Request.Form.Files.Count() > 1) //if we add imageM and imageSummernote
                    {
                        if (news.Id == 0)
                        {
                            byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                            news.Image = resultToByte;
                            _newsRepositoriy.Add(news);
                            news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            TempData[WebConstanta.Success] = "News Create successfully";
                        }
                        else
                        {
                            var objFromDB = await _newsRepositoriy.FirstOrDefault(u => u.Id == news.Id, isTracking: false);
                            if (files.Count > 0)
                            {
                                byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                                news.Image = resultToByte;
                            }
                            else
                            {
                                news.Image = objFromDB.Image;
                            }
                            objFromDB.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            TempData[WebConstanta.Success] = "News Update successfully";
                            _newsRepositoriy.Update(news);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(news.Description))
                        {
                            if (news.Id == 0)
                            {
                                byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                                news.Image = resultToByte;
                                news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                //    news.CreatedBy = await _userManager.GetUserAsync(User);
                                _newsRepositoriy.Add(news);
                                TempData[WebConstanta.Success] = "News Create successfully";
                            }
                            else
                            {
                                var objFromDB = await _newsRepositoriy.FirstOrDefault(u => u.Id == news.Id, isTracking: false);
                                if (files.Count > 0)
                                {
                                    byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                                    news.Image = resultToByte;
                                }
                                else
                                {
                                    news.Image = objFromDB.Image;
                                }
                                news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                TempData[WebConstanta.Success] = "News Update successfully";
                                _newsRepositoriy.Update(news);
                            }
                        }
                        else
                        {
                            if (news.Description.Contains("<img"))
                            {

                                if (news.Id == 0)
                                {
                                    byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                                    news.Image = resultToByte;
                                    news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                    _newsRepositoriy.Add(news);
                                    TempData[WebConstanta.Success] = "News Create successfully";
                                }
                                else
                                {
                                    var objFromDB = await _newsRepositoriy.FirstOrDefault(u => u.Id == news.Id, isTracking: false);
                                    if (files.Count > 0)
                                    {
                                        byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                                        news.Image = resultToByte;
                                    }
                                    else
                                    {
                                        news.Image = objFromDB.Image;
                                    }
                                    news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                    TempData[WebConstanta.Success] = "News Update successfully";
                                    _newsRepositoriy.Update(news);
                                }

                            }
                            else //if we don't any image
                            {
                                if (news.Id == 0)
                                {
                                    byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                                    news.Image = resultToByte;
                                    news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                    _newsRepositoriy.Add(news);
                                    TempData[WebConstanta.Success] = "News Create successfully";
                                }
                                else
                                {
                                    var objFromDB = await _newsRepositoriy.FirstOrDefault(u => u.Id == news.Id, isTracking: false);
                                    if (files.Count > 0)
                                    {
                                        byte[] resultToByte = await PhotoService.FileToByte(files); //get byte
                                        news.Image = resultToByte;
                                    }
                                    else
                                    {
                                        news.Image = objFromDB.Image;
                                    }
                                    news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                    TempData[WebConstanta.Success] = "News Update successfully";
                                    _newsRepositoriy.Update(news);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(news.Description))
                    {

                        if (news.Description.Contains("<img"))
                        {
                            var objFromDB = await _newsRepositoriy.FirstOrDefault(u => u.Id == news.Id, isTracking: false);
                            news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            news.Image = objFromDB.Image;
                            _newsRepositoriy.Update(news);
                            TempData[WebConstanta.Success] = "News Update successfully";
                        }
                        else
                        {
                            var objFromDB = await _newsRepositoriy.FirstOrDefault(u => u.Id == news.Id, isTracking: false);
                            news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            news.Image = objFromDB.Image;
                            _newsRepositoriy.Update(news);
                            TempData[WebConstanta.Success] = "News Update successfully";
                        }

                    }
                    else
                    {
                        var objFromDB = await _newsRepositoriy.FirstOrDefault(u => u.Id == news.Id, isTracking: false);
                        news.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        news.Image = objFromDB.Image;
                        _newsRepositoriy.Update(news);
                        TempData[WebConstanta.Success] = "News Update successfully";
                    }

                    




                }
                return RedirectToAction("Index"); //return to Action
            }


            return View();
        }

















        public async Task<IActionResult> UpateVision()
        {



            if (_newsRepositoriy.UpdateRange(newses))
            {
                TempData[WebConstanta.Success] = "News Visible successfully update";
                return RedirectToAction("Index");
            }
            TempData[WebConstanta.Error] = "News Visible Error!!!";
            return RedirectToAction("Index");
        }






        public async Task<IActionResult> Delete(int id)
        {

            _newsRepositoriy.Delete(new News() { Id = id });

            TempData[WebConstanta.Success] = "News Delete successfully";

            return RedirectToAction("Index"); //return to Action
        }


    }
}
