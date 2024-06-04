using AutoMapper;
using MongoDB.Driver;
using VillaApi.DataAccess.Data;
using VillaApi.DataAccess.Helper;
using VillaApi.Model;
using VillaApi.Model.modelDto;

namespace VillaApi.DataAccess.Service.VillaServices
{
    public class VillaService : IVillaService
    {

        private readonly ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

           public VillaService(IMapper mapper,ApplicationDbContext context)

           {
               _context = context;
               _response = new ApiResponse();
               _mapper = mapper;
           }

        public async Task<ApiResponse> CreateVillaAsync(VillaCreateDto villaCreate)
        {
            var res = await _context.Villas.Find(v => v.Name == villaCreate.Name).FirstOrDefaultAsync();
            if (res != null)return ApiResponse.ErrorException(HttpErrors.BadRequest, "Villa Already Exsit");
            var villa=_mapper.Map<Villa>(villaCreate);
            await _context.Villas.InsertOneAsync(villa);
            _response.Result = villa;
            return _response;
        }
        
        public async Task<ApiResponse> DeleteVillaAsync(Guid villaId)
        {
            var res = await _context.Villas.DeleteOneAsync(v => v.villaId == villaId);
            _response.Message = "Villa Deleted Success";
            return _response;
        }

        public async Task<ApiResponse> GetVillaAsync(Guid villaId)
        {
            var res = await _context.Villas.Find(v => v.villaId==villaId).FirstOrDefaultAsync();
            _response.Result = _mapper.Map<VillaDto>(res);
            return _response;
        }

        public async Task<ApiResponse> GetVillasAsync()
        {
            var res= await _context.Villas.Find(v=>true).ToListAsync();
           var villas= _mapper.Map<List<VillaDto>>(res);

            _response.Result = villas; 
            return _response;

        }

        public async Task<ApiResponse> UpdateVillaAsync(Guid villaId, VillaUpdateDto villaUpdateDto)
        {
            var res = await _context.Villas.Find(v => v.Name == villaUpdateDto.Name).FirstOrDefaultAsync();
            if (res == null) return ApiResponse.ErrorException(HttpErrors.NotFound, "Villa not Found");
            var villa = _mapper.Map<Villa>(villaUpdateDto);
            var result = await _context.Villas.ReplaceOneAsync(v => v.villaId == villaId, villa);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                _response.Result = result;
                return _response;
            }
            return ApiResponse.ErrorException(HttpErrors.BadRequest, "Error Occur");
        }
    }
}
