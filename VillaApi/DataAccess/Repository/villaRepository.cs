using VillaApi.DataAccess.Data;
using VillaApi.DataAccess.Repository.IRepository;
using VillaApi.Model;

namespace VillaApi.DataAccess.Repository
{
    public class villaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public villaRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public async Task<Villa> UpdateAsync(Villa villa)
        {
            villa.UpdatedDate = DateTime.Now;
           var vill= _db.villas.Update(villa);
            await _db.SaveChangesAsync();
            return villa;
        }
    }
}
