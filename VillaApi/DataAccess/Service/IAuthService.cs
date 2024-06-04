using VillaApi.DataAccess.Helper;
using VillaApi.Model.modelDto;

namespace VillaApi.DataAccess.Service
{
    public interface IAuthService
    {
        Task<ApiResponse> LoginAsync(LoginDto Input);
        Task<ApiResponse> RegisterAsync(RegisterDto Input);
    }
}
