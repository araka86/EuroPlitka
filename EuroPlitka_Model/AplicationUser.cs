using EuroPlitka_Model.ViewModels;
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


        [NotMapped]
    
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; } = string.Empty;

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; } = string.Empty;



     


    }
}
