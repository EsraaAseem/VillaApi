using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaApi.DataAccess.Data;
using VillaApi.DataAccess.Repository.IRepository;

namespace VillaApi.DataAccess.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbset;
        public Repository(ApplicationDbContext db)
        {
            this._db= db;
            this._dbset= db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
           await _dbset.AddAsync(entity);
           await SaveAsync();
        }
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperty = null)
        {
            IQueryable<T> query = _dbset;
            if(filter != null)
                query = query.Where(filter);
            if(includeProperty != null)
            {
                foreach(var includepro in includeProperty.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                    query=query.Include(includepro);
            }
            return await query.ToListAsync(); 
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperty = null, bool tracked = true)
        {
            IQueryable<T> query = _dbset;
            if (filter != null)
                query = query.Where(filter);
            if (!tracked)
                query = query.AsNoTracking();
            if (includeProperty != null)
            {
                foreach (var includepro in includeProperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includepro);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
             _dbset.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
           await _db.SaveChangesAsync();
        }
    }
}
