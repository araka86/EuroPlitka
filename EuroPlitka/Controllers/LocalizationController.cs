using EuroPlitka_DataAccess.Data;
using EuroPlitka_DataAccess.Repository.IRepository;
using EuroPlitka_Model;
using EuroPlitka_Model.ViewModels;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EuroPlitka.Controllers
{
    public class LocalizationController : Controller
    {
        private readonly IResourcesRepo _resourcesRepo;
        private readonly IViewRepo _viewRepo;
        private readonly IPageFileRepo _pageFileRepo;

        public LocalizationController(IResourcesRepo resourcesRepo, IViewRepo viewRepo, IPageFileRepo pageFileRepo)
        {
            _resourcesRepo = resourcesRepo;
            _viewRepo = viewRepo;
            _pageFileRepo = pageFileRepo;
        }




        public async Task<IActionResult> Index(LocalizationVM locVm, string lang, bool reset = false)
        {

            #region  Init 
            LocalizationVM localizationVM = new LocalizationVM()
            {
                resources = await _resourcesRepo.GetAll(includeProperties: ("View,Pagefille,Culture")),
                ViewList = await _resourcesRepo.DropdownLists(WebConstanta.ViewName),
                FileList = await _resourcesRepo.DropdownLists(WebConstanta.Pagefille)

            };
            //shut loop link
            foreach (var item in localizationVM.resources)
                item.Culture.Resources = null;
            #endregion

            if (reset == false)
            {
                if (locVm.ViewListFilter != null || locVm.FileList != null)
                {


                    if (locVm.ViewListFilter != "--Select View--")
                    {
                        localizationVM.resources = localizationVM.resources.Where(x => x.View.Id.ToString() == locVm.ViewListFilter);
                    }
                    if (locVm.FileListFilter != "--Select File--")
                    {

                        localizationVM.resources = localizationVM.resources.Where(x => x.Pagefille.Id.ToString() == locVm.FileListFilter);
                    }

                    if (lang == "UA")
                    {
                        localizationVM.resources = localizationVM.resources.Where(x => x.Culture.Name == "uk");
                    }
                    else if (lang == "ENG")
                    {
                        localizationVM.resources = localizationVM.resources.Where(x => x.Culture.Name == "en");
                    }

                    return View(localizationVM);
                }
                else
                {
                    if (lang == "UA")
                    {
                        localizationVM.resources = localizationVM.resources.Where(x => x.Culture.Name == "uk");
                    }
                    else if (lang =="ENG")
                    {
                        localizationVM.resources = localizationVM.resources.Where(x => x.Culture.Name == "en");
                    }

                    return View(localizationVM);

                }
            }

            ModelState.Clear();
            return View(localizationVM);
        }




        public async Task<IActionResult> IndexView() => View(await _viewRepo.GetAll());
        public async Task<IActionResult> IndexPageFile() => View(await _pageFileRepo.GetAll());
        #region CRUD_VIEW 
        public async Task<IActionResult> UpsertView(int? id)
        {
            var nameView = new Europlitkaview();

            if (id == null)
            {

                return View(nameView);
            }
            else
            {
                nameView = await _viewRepo.FirstOrDefault(x => x.Id == id);
                return View(nameView);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpsertView(Europlitkaview europlitkaview)
        {
            if (ModelState.IsValid)
            {
                if (europlitkaview.Id == 0)
                {
                    //////////////CREATE//////////////////
                    var chkResult = await _viewRepo.FirstOrDefault(x => x.Name == europlitkaview.Name);
                    if (chkResult == null)
                    {
                        _viewRepo.Add(europlitkaview);
                        TempData[WebConstanta.Success] = "View Create successfully";
                        return RedirectToAction("IndexView");
                    }
                    else
                    {
                        TempData[WebConstanta.Error] = "Error create, view exist!!!!";
                    }
                }
                else
                {
                    //////////////UPDATE//////////////////

                    var chkResult = await _viewRepo.FirstOrDefault(x => x.Name == europlitkaview.Name);
                    if (chkResult == null)
                    {
                        _viewRepo.Update(europlitkaview);
                        TempData[WebConstanta.Success] = "View Update successfully";
                    }
                    else
                    {
                        TempData[WebConstanta.Error] = "Error update, view exist!!!!";
                    }
                }
            }

            return View(europlitkaview);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteView(int? id)
        {
            var getId = await _viewRepo.FirstOrDefault(x => x.Id == id);
            if (getId == null)
            {
                TempData[WebConstanta.Error] = "Error delete view!!!!";
                return View(getId);
            }
            else
            {
                _viewRepo.Delete(getId);
                TempData[WebConstanta.Success] = "View Delete successfully";
                return RedirectToAction("IndexView");
            }
        }
        #endregion

        #region CRUD_Page_File

        public async Task<IActionResult> UpsertPageFiele(int? id)
        {
            var nameView = new Pagefille();

            if (id == null)
            {

                return View(nameView);
            }
            else
            {
                nameView = await _pageFileRepo.FirstOrDefault(x => x.Id == id);
                return View(nameView);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpsertPageFiele(Pagefille pagefille)
        {
            if (ModelState.IsValid)
            {
                if (pagefille.Id == 0)
                {
                    //////////////CREATE//////////////////
                    var chkResult = await _pageFileRepo.FirstOrDefault(x => x.Filename == pagefille.Filename);
                    if (chkResult == null)
                    {
                        _pageFileRepo.Add(pagefille);
                        TempData[WebConstanta.Success] = "PageFile Create successfully";
                        return RedirectToAction("IndexPageFile");
                    }
                    else
                    {
                        TempData[WebConstanta.Error] = "Error create, pagefille exist!!!!";
                    }
                }
                else
                {
                    //////////////UPDATE//////////////////

                    var chkResult = await _pageFileRepo.FirstOrDefault(x => x.Filename == pagefille.Filename);
                    if (chkResult == null)
                    {
                        _pageFileRepo.Update(pagefille);
                        TempData[WebConstanta.Success] = "Pagefille Update successfully";
                    }
                    else
                    {
                        TempData[WebConstanta.Error] = "Error update, Pagefille exist!!!!";
                    }
                }
            }

            return View(pagefille);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePageFiele(int? id)
        {
            var getobj = await _pageFileRepo.FirstOrDefault(x => x.Id == id);
            if (getobj == null)
            {
                TempData[WebConstanta.Error] = "Error delete PageFiele!!!!";
                return View(getobj);
            }
            else
            {
                _pageFileRepo.Delete(getobj);
                TempData[WebConstanta.Success] = "PageFiele Delete successfully";
                return RedirectToAction("IndexPageFile");
            }
        }

        #endregion


        public async Task<IActionResult> UpsertResources(int? id)
        {
            var nameResorces = new LocalizationVM()
            {
                resource = new Resource() { Culture = new Culture()},
                ViewList = await _resourcesRepo.DropdownLists(WebConstanta.ViewName),
                FileList = await _resourcesRepo.DropdownLists(WebConstanta.Pagefille)
            };

            if (id == null)
            {

                return View(nameResorces);
            }
            else
            {
                nameResorces.resource = await _resourcesRepo.FirstOrDefault(x => x.Id == id, includeProperties: ("View,Pagefille,Culture"));
                if (nameResorces.resource.Id == null)
                {
                    TempData[WebConstanta.Error] = "Not found!!!";
                    return NotFound();
                }

                return View(nameResorces);
            }
        }



        [HttpPost]
        public async Task<IActionResult> UpsertResources(LocalizationVM?  localizationVM, string? flexRadioDefault)
        {

            var findIdLang = await _resourcesRepo.FirstOrDefault(x=>x.Culture.Name == flexRadioDefault,includeProperties:("Culture"), isTracking: false);


            if (ModelState.IsValid)
            {
                if (localizationVM.resource.Id == 0)
                {
                    //////////////CREATE//////////////////
                    var chkResult = await _resourcesRepo.FirstOrDefault(x => x.Param == localizationVM.resource.Param); //find the same word
                    if (chkResult == null || (chkResult!=null && chkResult.CultureId != findIdLang.CultureId) )
                    {
                        localizationVM.resource.CultureId = findIdLang.CultureId;
                        _resourcesRepo.Add(localizationVM.resource);
                        TempData[WebConstanta.Success] = "Resource was Add successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[WebConstanta.Error] = "Error added, resources is exist!!!!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //////////////UPDATE//////////////////

                    var chkResult = await _resourcesRepo.FirstOrDefault(x => x.Id == localizationVM.resource.Id, isTracking: false);
                    if (chkResult != null)
                    {
                        localizationVM.resource.CultureId = findIdLang.CultureId;
                        _resourcesRepo.Update(localizationVM.resource);
                        TempData[WebConstanta.Success] = "Resources was Update successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData[WebConstanta.Error] = "Error update, Resource is exist!!!!";
                    }
                }
            }

            return View(localizationVM.resource);



        }



        }
}
