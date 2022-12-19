using Microsoft.AspNetCore.Http;

namespace EuroPlitka_Model.ViewModels
{
    public class EditUserVM
    {
        public string Id { get; set; } 
        public string? FullName { get; set; } = string.Empty;

        public byte[]? imgUserAva { get; set; }

        public string? StreetAddress { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? ZipCode { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }



    }
}
