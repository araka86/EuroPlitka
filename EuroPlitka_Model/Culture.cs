using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroPlitka_Model
{
    public class Culture
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Resource> Resources { get; set; }
        public Culture()
        {
            Resources = new List<Resource>();
        }
    }
}
