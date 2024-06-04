using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Controllers;
using VillaApi.DataAccess.Helper;
using VillaApi.DataAccess.Service.VillaServices;
using VillaApi.Model;
using VillaApi.Model.modelDto;

namespace VillaApiTest
{
    public class VillaNumberController_Test
    {
        private readonly IVillaNumberService _villaRepo;
        private readonly VillaNumberController _villaController;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;
        public VillaNumberController_Test()
        {
            _villaRepo = A.Fake<IVillaNumberService>();
            _mapper = A.Fake<IMapper>();
            _villaController = new VillaNumberController(_villaRepo);
            _response = new();
        }


        [Fact]
        public async Task GetVillas_ReturnAllVillas()
        {
            //arr
            var villas = new List<VillaNumber>();
            villas.Add(getVillaNumber());
            var villaDtos = new List<VillaNumberDto>();
            villaDtos.Add(getVillaNumberDto());
            A.CallTo(() => _mapper.Map<List<VillaNumberDto>>(villas)).Returns(villaDtos);
            _response.Result = villaDtos;

            A.CallTo(() => _villaRepo.GetVillasNumberAsync()).Returns(_response);

            // Act
            var result = await _villaController.getVillas();
            //
            var status = 200;
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(villaDtos, apiResponse.Result);
            Assert.Equal(status, (int)apiResponse.Status);
        }

        [Fact]
        public async Task CreateVilla_ReturnAddVilla()
        {
            //arr
            var villa = getVillaNumber();
            var villaCreate = VillaNumberCreate();
            _response.Result = villa;
            A.CallTo(() => _villaRepo.CreateVillaNumberAsync(villaCreate))
                       .Returns(_response);
            //act
            var res = await _villaController.createVilla(villaCreate);
            //acc

            //ass
            var okResult = Assert.IsType<OkObjectResult>(res.Result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            var result = Assert.IsType<VillaNumber>(apiResponse.Result);
            Assert.Equal(villa, result);
        }


       [Fact]
        public async Task UpdateVilla_ReturnUpdateVilla()
        {
            //arr
            var villa = getVillaNumber();
            var villaUpdate = VillaUpdate();
            _response.Result = villa;
            A.CallTo(() => _villaRepo.UpdateVillaNumberAsync( villaUpdate))
                       .Returns(_response);
            //actu

            var res = await _villaController.UpdateVilla(villaUpdate);
            //acc
            //assert
            var actionResult = Assert.IsType<ActionResult<ApiResponse>>(res);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            var UpdateVilla = Assert.IsType<VillaNumber>(apiResponse.Result);
        }
        

        [Fact]
        public async Task DeleteVilla_ReturnDeleteVilla()
        {
            //arr
            var villa = getVillaNumber();
            _response.Message = "Villa Deleted Success";
            A.CallTo(() => _villaRepo.DeleteVillaAsync(villa.villaNbId)).Returns(_response);
            //act
            var res = await _villaController.DeleteVilla(villa.villaNbId);
            //acc
            var message = "Villa Deleted Success";
            //assert
            var okResult = Assert.IsType<OkObjectResult>(res);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(message, apiResponse.Message);

        }

       
        private VillaNumber getVillaNumber()
        {
            var villaNumer = new VillaNumber
            {
                villaNbId = 2,
                SpecialDetails = "available",
                CreatedDate = DateTime.Parse("2024-03-25T13:49:20.1864323"),
                UpdatedDate = DateTime.Parse("2024-03-25T13:49:20.1864486"),
               VillaId = Guid.Parse("83facf7e-8057-42dc-decc-08dc47b2aef3"),
            };
            return villaNumer;
        }
        private VillaNumberDto getVillaNumberDto()
        {
            var villaNumer = new VillaNumberDto
            {
                villaNbId = 2,
                SpecialDetails = "available",
                CreatedDate = DateTime.Parse("2024-03-25T13:49:20.1864323"),
                UpdatedDate = DateTime.Parse("2024-03-25T13:49:20.1864486"),
                villa = new Villa
                {
                    villaId = Guid.Parse("83facf7e-8057-42dc-decc-08dc47b2aef3"),
                    Name = "newVilla",
                    Details = "very Page",
                    Rate = 4,
                    Sqft = 12,
                    ImageUrl = "string",
                    Amenity = "jhhl",
                    CreatedDate = DateTime.Parse("2024-03-19T03:19:54.4427398"),
                    UpdatedDate = DateTime.Parse("2024-03-19T03:19:54.4429032")

                }
            };
            return villaNumer;
        }
        private VillaNumberCreateDto VillaNumberCreate()
        {
            var villa = new VillaNumberCreateDto
            {
                villaNbId=3,
                CreatedDate = DateTime.Parse("2024-03-19T03:19:54.4427398"),
                SpecialDetails="New Details",
                VillaId = Guid.Parse("83facf7e-8057-42dc-decc-08dc47b2aef3"),
            };
            return villa;
        }
        private VillaNumberUpdateDto VillaUpdate()
        {
            var villa = new VillaNumberUpdateDto
            {
                villaNbId = 3,
                UpdatedDate = DateTime.Parse("2024-03-19T03:19:54.4427398"),
                SpecialDetails = "New Details",
                VillaId = Guid.Parse("83facf7e-8057-42dc-decc-08dc47b2aef3"),
            };
            return villa;
        }
    
    }
}
