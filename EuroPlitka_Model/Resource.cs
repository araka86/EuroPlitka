using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuroPlitka_Model
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }
        public string Param { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;


        public int CultureId { get; set; }
        [ForeignKey("CultureId")]
        public virtual Culture? Culture { get; set; }


        public int EuroplitkaviewId { get; set; }
        [ForeignKey("EuroplitkaviewId")]
        public virtual Europlitkaview? View { get; set; }


        public int PagefilleId { get; set; }
        [ForeignKey("PagefilleId")]
        public virtual Pagefille? Pagefille { get; set; }




    }
}
