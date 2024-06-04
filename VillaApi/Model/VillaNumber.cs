using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaApi.Model
{
    public class VillaNumber
    {
        [BsonId]
        public int villaNbId { get; set; }
        
        public string? SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid VillaId { get; set; }
    }
}
