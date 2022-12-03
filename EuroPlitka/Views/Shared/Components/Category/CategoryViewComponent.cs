using EuroPlitka_DataAccess.Repository.IReposotory;
using Microsoft.AspNetCore.Mvc;

namespace EuroPlitka.Views.Shared.Components.Category
{
    public class CategoryViewComponent : ViewComponent
    {

        private readonly ICategoryRepository _catRepo;
        public CategoryViewComponent(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }

        //IViewComponentResult - view
        //IContentTypeHttpResult  - content
        //HtmlContentViewComponentResult - html

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _catRepo.GetAll();
            return View(result);
        }
    }
}
