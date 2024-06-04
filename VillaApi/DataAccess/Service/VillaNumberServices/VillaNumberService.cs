using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VillaApi.DataAccess.Data;
using VillaApi.DataAccess.Helper;
using VillaApi.Model;
using VillaApi.Model.Helper;
using VillaApi.Model.modelDto;

namespace VillaApi.DataAccess.Service.VillaServices
{
    public class VillaNumberService : IVillaNumberService
    {

        private readonly ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

           public VillaNumberService(IMapper mapper,IOptions<MongoDbConfiguration> context)

           {
               _context = new ApplicationDbContext(context);
               _response = new ApiResponse();
               _mapper = mapper;
           }

        public async Task<ApiResponse> CreateVillaNumberAsync(VillaNumberCreateDto villaCreate)
        {
            var villa=_mapper.Map<VillaNumber>(villaCreate);
            await _context.VillasNumber.InsertOneAsync(villa);
            _response.Result = villa;
            return _response;
        }

        public async Task<ApiResponse> GetVillaNumberAsync(int villaId)
        {
            var villaNumber = await _context.VillasNumber.Find(v => v.villaNbId==villaId).FirstOrDefaultAsync();
            var res=_mapper.Map<VillaNumberDto>(villaNumber);
            if(res.VillaId!=null)
               res.villa=  await _context.Villas.Find(v => v.villaId == villaNumber.VillaId).FirstOrDefaultAsync();
            _response.Result = res;
            return _response;
        }

        public async Task<ApiResponse> GetVillasNumberAsync()
        {
            var villaNumber = await _context.VillasNumber.Find(v => true).ToListAsync();
            var res = _mapper.Map<List<VillaNumberDto>>(villaNumber);
            foreach( var villaNumb in res)
            if (villaNumb.VillaId != null)
                villaNumb.villa = await _context.Villas.Find(v => v.villaId == villaNumb.VillaId).FirstOrDefaultAsync();
            _response.Result = res;
            return _response;

        }

      

        public async Task<ApiResponse> UpdateVillaNumberAsync( VillaNumberUpdateDto villaUpdateDto)
        {
            var res = await _context.VillasNumber.Find(v => v.villaNbId ==villaUpdateDto.villaNbId).FirstOrDefaultAsync();
            if (res == null) return ApiResponse.ErrorException(HttpErrors.NotFound, "Villa not Found");
            var villa = _mapper.Map<VillaNumber>(villaUpdateDto);
            var result =await  _context.VillasNumber.ReplaceOneAsync(v => v.villaNbId == villaUpdateDto.villaNbId, villa);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                _response.Result = result;
                return _response;
            }
            return ApiResponse.ErrorException(HttpErrors.BadRequest, "Error Occur");
        }
        public async Task<ApiResponse> DeleteVillaAsync(int villaId)
        {
            var res = await _context.VillasNumber.DeleteOneAsync(v => v.villaNbId == villaId);
            _response.Message = "Villa Deleted Success";
            return _response;
        }


    }
}
