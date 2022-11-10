using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroPlitka_Model
{
    public class AplicationUser :IdentityUser
    {

        [Required]
        public string FullName { get; set; } = "Name";

        public byte[]? imgUserAva {  get; set; }
     
        public string? StreetAddress { get; set; } = string.Empty;
     
        public string? City { get; set; } = string.Empty;

        public string? Description {  get; set; } = string.Empty;

        [NotMapped]
        public string? State { get; set; } = string.Empty;
        [NotMapped] 
        public string? PostalCode { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? Image { get; set; }




    }
}
