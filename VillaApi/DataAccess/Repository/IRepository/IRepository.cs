using System.Linq.Expressions;

namespace VillaApi.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
         Task<List<T>> GetAllAsync(Expression<Func<T,bool>>?filter=null,string?includeProperty=null);
         Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperty = null,bool tracked=true);
        Task CreateAsync(T entity);
        Task  RemoveAsync(T entity);
        Task SaveAsync();
    }
}
