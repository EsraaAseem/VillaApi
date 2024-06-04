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
    public class VillaController_Test
    {
        private readonly IVillaService _villaRepo;
        private readonly VillaController _villaController;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;
        public VillaController_Test()
        {
            _villaRepo = A.Fake<IVillaService>();
            _mapper = A.Fake<IMapper>();
            _villaController = new VillaController(_villaRepo);
            _response = new ApiResponse();
        }
       [Fact]
        public async Task GetVillas_ReturnAllVillas()
        {
            var villas = new List<Villa>(); 
            villas.Add(getVilla());
            var villaDtos = new List<VillaDto>();
            villaDtos.Add(getVillaDto()); 
            _response.Result= villas;
            A.CallTo(() => _villaRepo.GetVillasAsync()).Returns(_response);
            A.CallTo(() => _mapper.Map<List<VillaDto>>(villas)).Returns(villaDtos);

            // Act
            var result = await _villaController.getVillas();
            //
            var status = 200;
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(villas, apiResponse.Result);
            Assert.Equal(status,(int)apiResponse.Status);
        }

        [Fact]
        public async Task GetVillas_WithNoVillas_ReturnResPonseWithNullResult()
        {
            var villas = new List<Villa>();
            var villaDtos = new List<VillaDto>();
            _response.Result = villas;
            A.CallTo(() => _villaRepo.GetVillasAsync()).Returns(_response);
            A.CallTo(() => _mapper.Map<List<VillaDto>>(villas)).Returns(null);
            // Act
            var result = await _villaController.getVillas();
            //accept
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(villaDtos, apiResponse.Result);
        }

          [Fact]
          public async Task CreateVilla_ReturnAddVilla()
          {
            //arr
              var villa = GetVilla();
              var villaCreate = VillaCreate();
              var villaRes = new Villa();
              _response.Result =villa;
            A.CallTo(() => _villaRepo.CreateVillaAsync(villaCreate))
                       .Returns(_response);
            //act
              var res = await _villaController.createVilla(villaCreate);
            //acc
           
            //ass
              var okResult = Assert.IsType<OkObjectResult>(res.Result);
            var apiResponse=Assert.IsType<ApiResponse>(okResult.Value);
            var result = Assert.IsType<Villa>(apiResponse.Result);
            Assert.Equal(villa, result);
          }
        
          
          [Fact]
          public async Task UpdateVilla_ReturnUpdateVilla()
          {
            //arr
              var villa = GetVilla();
              var villaUpdate = VillaUpdate();
              var villaRes = new Villa();
            _response.Result =villa;
              A.CallTo(() => _villaRepo.UpdateVillaAsync(villa.villaId,villaUpdate))
                         .Returns(_response);
              A.CallTo(() => _mapper.Map<Villa>(villaUpdate)).Returns(villa);
            //actu

              var res = await _villaController.UpdateVilla(villa.villaId,villaUpdate);
            //acc
            //assert
              var actionResult = Assert.IsType<ActionResult<ApiResponse>>(res);
              var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
              var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
              var UpdateVilla = Assert.IsType<Villa>(apiResponse.Result);
          }
        
         
          [Fact]
          public async Task DeleteVilla_ReturnDeleteVilla()
          {
            //arr
              var villa = GetVilla();
              var villaRes = new Villa();
              _response.Message= "Villa Deleted Success";
              A.CallTo(() => _villaRepo.DeleteVillaAsync(villa.villaId)).Returns(_response);
            //act
              var res = await _villaController.DeleteVilla(villa.villaId);
            //acc
            var message = "Villa Deleted Success";
            //assert
            var actionResult = Assert.IsType<ActionResult<ApiResponse>>(res);
              var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
              var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(message, apiResponse.Message);

        }
        private Villa getVilla()
        {
            var villa = new Villa
            {

                Name = "Premium Pool Villa",
                Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Rate = 300,
                Sqft = 550,
                ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                Amenity = ""
            };
            return villa;
        }
        private VillaDto getVillaDto()
        {
            var villa = new VillaDto
            {

                Name = "Premium Pool Villa",
                Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Rate = 300,
                Sqft = 550,
                ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                Amenity = ""
            };
            return villa;
        }
        private VillaCreateDto VillaCreate()
        {
            var villa = new VillaCreateDto
            {
                Name="Shik_Zayad",
                Details="modern villa",
                Sqft=5,
                ImageUrl="https:://villa.com",
                Amenity="modr",
                Rate=4,
                
            };
            return villa;
        }
        private VillaUpdateDto VillaUpdate()
        {
            var villa = new VillaUpdateDto
            {
                Name = "Shiio_Zayad",
                Details = "modern villa",
                Sqft = 5,
                ImageUrl = "https:://villa.com",
                Amenity = "modr",
                Rate = 4,

            };
            return villa;
        }
        private Villa GetVilla()
        {
            var villa = new Villa
            {
                Name = "Shik_Zayad",
                Details = "modern villa",
                Sqft = 5,
                ImageUrl = "https:://villa.com",
                Amenity = "modr",
                Rate = 4,

            };
            return villa;
        }

    
    }
}
