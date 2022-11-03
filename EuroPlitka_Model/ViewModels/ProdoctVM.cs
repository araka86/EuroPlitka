using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Buffers.Text;
using System.Drawing;

namespace EuroPlitka_Model.ViewModels
{
    public class ProdoctVM
    {
        public Product? Product { get; set; }
      //  public IFormFile FileImage { get; set; }
        public Image?  image { get; set; }

        public string? Base64 { get; set; }

       
        public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
        public IEnumerable<SelectListItem>? ProductTypeSelectList { get; set; }
    }
}
