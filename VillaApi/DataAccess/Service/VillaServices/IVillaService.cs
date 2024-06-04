using VillaApi.DataAccess.Helper;
using VillaApi.Model.modelDto;

namespace VillaApi.DataAccess.Service.VillaServices
{
    public interface IVillaService
    {
        Task<ApiResponse> GetVillasAsync();
        Task<ApiResponse> GetVillaAsync(Guid villaId);
        Task<ApiResponse> CreateVillaAsync(VillaCreateDto villaCreate);
        Task<ApiResponse> UpdateVillaAsync(Guid villaId,VillaUpdateDto villaUpdateDto);
        Task<ApiResponse> DeleteVillaAsync(Guid villaId);

    }
}
