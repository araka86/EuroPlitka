using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroPlitka_Model
{
    public class Basket
    {

        [Key]
        public int Id { get; set; }

        
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int Sqft { get; set; }


        public string? CreatedByUserId { get; set; } 
        [ForeignKey("CreatedByUserId")]
        public AplicationUser? CreatedBy { get; set; }


        public DateTime Data { get; set; }


    }
}
