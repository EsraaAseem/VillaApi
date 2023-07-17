using System.ComponentModel.DataAnnotations.Schema;

namespace VillaApi.Model
{
    public class VillaNumber
    {
        public int villaNbId { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [ForeignKey("VillaId")]
        public Guid VillaId { get; set; }
        public Villa villa { get; set; }
    }
}
