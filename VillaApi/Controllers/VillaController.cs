using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaApi.DataAccess.Helper;
using VillaApi.DataAccess.Repository.IRepository;
using VillaApi.Model;
using VillaApi.Model.modelDto;

namespace VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("text/json")]

    public class VillaController : ControllerBase
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        protected readonly ApiResponse _response;
        public VillaController(IVillaRepository villaRepository,IMapper mapper)
        {
            this._villaRepository = villaRepository;
            this._mapper = mapper;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>>getVillas()
        {
            
             try
             {
                  var villas = await _villaRepository.GetAllAsync();
                  _response.HttpStatusCode = HttpStatusCode.OK;
                  _response.Result = _mapper.Map<List<VillaDto>>(villas);
                  return Ok(_response);
             }
              catch (Exception ex)
             {
                  _response.isSuccess = false;
                  _response.ErrorMessages=new List<string>() { ex.ToString()};
                return _response;
             }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>>createVilla(VillaCreateDto villaCreate)
        {

            try
            {
                var villaexit=await _villaRepository.GetFirstOrDefaultAsync(u=>u.Name==villaCreate.Name);
                if(villaexit!=null)
                {
                    _response.HttpStatusCode=HttpStatusCode.BadRequest;
                    _response.isSuccess = false;
                    _response.ErrorMessages = new List<string>() { "this villa is already exit" };
                    return _response;
                }
                villaCreate.CreatedDate = DateTime.Now;
                villaCreate.UpdatedDate = DateTime.Now;
                var villa =  _mapper.Map<Villa>(villaCreate);
                  await _villaRepository.CreateAsync(villa);
                _response.Result = villa;
                _response.HttpStatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.isSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdateVilla(VillaUpdateDto villaupdate)
        {

            try
            {
                var villaexit = await _villaRepository.GetFirstOrDefaultAsync(u => u.villaId==villaupdate.villaId);
                if (villaexit == null)
                {
                    _response.HttpStatusCode = HttpStatusCode.NotFound;
                    _response.isSuccess = false;
                    _response.ErrorMessages = new List<string>() { "this villa not found in the databasse" };
                    return _response;
                }
               // villaCreate.UpdatedDate = DateTime.Now;
                var villa = _mapper.Map<Villa>(villaupdate);
                await _villaRepository.UpdateAsync(villa);
                _response.Result = villa;
                _response.HttpStatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                _response.isSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
    }
}
