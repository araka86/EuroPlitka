using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroPlitka_Model
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        public string? CreatedByUserId { get; set; } 
        [ForeignKey("CreatedByUserId")]
        public AplicationUser? CreatedBy { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } //дата заказа

      
        [Required]
        public double FinalOrderTotal { get; set; } //общая цена

        
        public int СountItem { get; set; }
    

        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? StreetAddress { get; set; } = string.Empty;
        [Required]
        public string? City { get; set; } = string.Empty;
        [Required]
        public string? State { get; set; } = string.Empty;
        [Required]
        public string? PostalCode { get; set; } = string.Empty;
        [Required]
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool OrderFactura { get; set; } = false;
        public bool Delivery { get; set; } = false;
    }
}
