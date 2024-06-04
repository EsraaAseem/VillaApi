using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VillaApi.Model.modelDto
{
    public class VillaNumberCreateDto
    {
        public int villaNbId { get; set; }
        public string SpecialDetails { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public Guid VillaId { get; set; }
    }
}
