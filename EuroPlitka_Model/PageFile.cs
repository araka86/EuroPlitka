using System.ComponentModel.DataAnnotations;

namespace EuroPlitka_Model
{
    public class Pagefille
    {
        [Key]
        public int Id { get; set; }

        public string Filename { get; set; } = string.Empty;
    }
}
