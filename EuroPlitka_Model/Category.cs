using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_Model
{
    public class Category
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order for category must be greather than 0")]
        public int DisplayOrder { get; set; }


    }
}
