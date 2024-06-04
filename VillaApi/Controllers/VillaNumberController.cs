using Microsoft.AspNetCore.Mvc;
using VillaApi.DataAccess.Helper;
using VillaApi.DataAccess.Service.VillaServices;
using VillaApi.Model.modelDto;

namespace VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("text/json")]
    public class VillaNumberController : ControllerBase
    {
        private readonly IVillaNumberService _villaService;
        public VillaNumberController(IVillaNumberService villaService)
        {
            _villaService = villaService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> getVillas()
        {
            var villas = await _villaService.GetVillasNumberAsync();
            return Ok(villas);
        }
        [HttpGet("{villaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> getVilla([FromRoute] int villaId)
        {
            var res = await _villaService.GetVillaNumberAsync(villaId);
            return Ok(res);
        }
        [HttpDelete("{villaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteVilla([FromRoute] int villaId)
        {
            var res = await _villaService.DeleteVillaAsync(villaId);
            return Ok(res);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> createVilla(VillaNumberCreateDto villaCreate)
        {

            if (!ModelState.IsValid)
                return ApiResponse.ErrorException(HttpErrors.BadRequest, "this model not vaild");
            var villa = await _villaService.CreateVillaNumberAsync(villaCreate);
            return Ok(villa);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateVilla(VillaNumberUpdateDto villaupdate)
        {

            var res = await _villaService.UpdateVillaNumberAsync(villaupdate);
            return Ok(res);
        }
        
    }
}
