using VillaApi.Model;

namespace VillaApi.DataAccess.Repository.IRepository
{
    public interface IVillaRepository:IRepository<Villa>
    {
        Task <Villa> UpdateAsync(Villa villa);
    }
}
