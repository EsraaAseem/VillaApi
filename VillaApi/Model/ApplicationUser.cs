using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VillaApi.Model
{
    [CollectionName("users")]

    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        [Required]
        public string Name { get; set; }
        
    }
}
