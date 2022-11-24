using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EuroPlitka_Model
{
    public class News
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string ShortDescription { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public byte[]? Image { get; set; }

       

    }
}
