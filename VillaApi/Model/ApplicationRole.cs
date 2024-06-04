using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace VillaApi.Model
{
    [CollectionName("roles")]

    public class ApplicationRole:MongoIdentityRole<Guid>
    {
    }
}
