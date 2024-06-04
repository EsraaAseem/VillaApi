using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VillaApi.Model.modelDto
{
    public class VillaNumberDto
    {
        public int villaNbId { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [JsonIgnore]
        public Guid VillaId { get; set; }
        public Villa villa { get; set; }
    }
}
