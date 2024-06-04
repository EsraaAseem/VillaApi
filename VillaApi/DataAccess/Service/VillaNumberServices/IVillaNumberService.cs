using VillaApi.DataAccess.Helper;
using VillaApi.Model.modelDto;

namespace VillaApi.DataAccess.Service.VillaServices
{
    public interface IVillaNumberService
    {
        Task<ApiResponse> GetVillasNumberAsync();
        Task<ApiResponse> GetVillaNumberAsync(int villaId);
        Task<ApiResponse> CreateVillaNumberAsync(VillaNumberCreateDto villaCreate);
        Task<ApiResponse> UpdateVillaNumberAsync(VillaNumberUpdateDto villaUpdateDto);
        Task<ApiResponse> DeleteVillaAsync(int villaId);

    }
}
