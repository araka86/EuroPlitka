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

        public string? CreatedByUserId { get; set; } //указано кто из админов создал эту запись (лог, кто создал заказ)
        [ForeignKey("CreatedByUserId")]
        public AplicationUser? CreatedBy { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } //дата заказа

        public DateTime ShippingDate { get; set; }
        [Required]
        public double FinalOrderTotal { get; set; } //общая цена

        
        public int countItem { get; set; }
    

        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? StreetAddress { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? PostalCode { get; set; }
        [Required]
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
