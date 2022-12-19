using System.ComponentModel.DataAnnotations;

namespace EuroPlitka_Model
{
    public class Europlitkaview
    {
        [Key]
        public int  Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }

}
