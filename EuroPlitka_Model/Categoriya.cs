using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroPlitka_Model
{
    public class Categoriya
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

 
        [ForeignKey("CategoriyaID")]    
        public virtual  Categoriya? categoriyacalss {  get; set; }
     

    }
}
