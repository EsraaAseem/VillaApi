using Microsoft.AspNetCore.Mvc;
using VillaApi.DataAccess.Helper;
using VillaApi.DataAccess.Service.VillaServices;
using VillaApi.Model.modelDto;

namespace VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("text/json")]

    public class VillaController : ControllerBase
    {
        private readonly IVillaService _villaService;
        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> getVillas()
        {
            var villas = await _villaService.GetVillasAsync();
            return Ok(villas);
        }
        [HttpGet("{villaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> getVilla([FromRoute] Guid villaId)
        {
            var res = await _villaService.GetVillaAsync(villaId);
            return Ok(res);
        }
        [HttpDelete("{villaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteVilla([FromRoute] Guid villaId)
        {
            var res = await _villaService.DeleteVillaAsync(villaId);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> createVilla(VillaCreateDto villaCreate)
        {

            if (!ModelState.IsValid)
                return ApiResponse.ErrorException(HttpErrors.BadRequest, "this model not vaild");
            var villa = await _villaService.CreateVillaAsync(villaCreate);
            return Ok(villa);
        }

        [HttpPut("{villaId}")]
        public async Task<ActionResult<ApiResponse>> UpdateVilla(Guid villaId, VillaUpdateDto villaupdate)
        {
            var res = await _villaService.UpdateVillaAsync(villaId, villaupdate);
            return Ok(res);
        }
        /*  [HttpDelete]
          public async Task<ActionResult<ApiResponse>> DeleteVilla(Guid villaId)
          {
              var villaexit = await _villaRepository.GetFirstOrDefaultAsync(u => u.villaId ==villaId);
              if (villaexit == null)
                 return ApiResponse.ErrorException(HttpErrors.NotFound, "this villa is  not found in the databasse");
                  await _villaRepository.RemoveAsync(villaexit);
                  return Ok(_response);
          }*/
    }
}
