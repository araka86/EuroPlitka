using Microsoft.AspNetCore.Mvc.Rendering;

namespace EuroPlitka_Model.ViewModels
{
    public class LocalizationVM
    {
        public IEnumerable<Resource>? resources { get; set; }
        public Resource? resource { get; set; }

     


        public string? ViewListFilter { get; set; }
        public string? FileListFilter { get; set; }

        public IEnumerable<SelectListItem>? ViewList { get; set; }
        public IEnumerable<SelectListItem>? FileList { get; set; }
       
    }
}
