using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        [Required]
        public DateTime DateTime { get; set; }


        public bool IsMainMenu { get; set; } = false;

        [NotMapped]
        public string checkedState { get; set; } = string.Empty;

        [NotMapped]
        public bool IsFirst { get; set; }

        [NotMapped]
        public string? NameUser { get; set; }

        public string? CreatedByUserId { get; set; } 
        [ForeignKey("CreatedByUserId")]
        public AplicationUser? CreatedBy { get; set; }



    }
}
