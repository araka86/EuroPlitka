using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_Model
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Pick the Name")]
        [Required]
        [StringLength(20, ErrorMessage = "Name length can't be more than 20.")]
        public string? Name { get; set; }

    }
}
