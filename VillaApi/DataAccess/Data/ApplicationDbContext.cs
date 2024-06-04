using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VillaApi.Model;
using VillaApi.Model.Helper;

namespace VillaApi.DataAccess.Data
{
    public class ApplicationDbContext
    {
      private readonly  IMongoDatabase _database;
        public ApplicationDbContext(IOptions<MongoDbConfiguration> settings)
        {
            var connection = new MongoClient(settings.Value.ConnectionString);
            _database=connection.GetDatabase(settings.Value.DatabaseName);
        }
        public IMongoCollection<Villa> Villas => _database.GetCollection<Villa>("Villas");
        public IMongoCollection<VillaNumber> VillasNumber => _database.GetCollection<VillaNumber>("VillasNumber");


        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              base.OnModelCreating(modelBuilder);

          }*/
    }
}
