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
        [DisplayName("Name UA")]
        [Required]
        public string? NameUa { get; set; }

        [DisplayName("Name ENG")]
        [Required]      
        public string? NameEng { get; set; }


    }
}
