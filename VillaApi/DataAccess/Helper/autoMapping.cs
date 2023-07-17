using AutoMapper;
using VillaApi.Model.modelDto;
using VillaApi.Model;

namespace VillaApi.DataAccess.Helper
{
    public class autoMapping:Profile
    {
        public autoMapping()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();


        }
    }
}
