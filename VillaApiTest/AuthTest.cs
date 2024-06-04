using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Controllers;
using VillaApi.DataAccess.Helper;
using VillaApi.DataAccess.Service;
using VillaApi.Model.modelDto;

namespace VillaApiTest
{
    public class AuthTest
    {
        private readonly IAuthService _authService;
        private readonly AuthController _authController;
        private readonly ApiResponse _response;
        public AuthTest()
        {
            _authService=A.Fake<IAuthService>();
            _authController = new AuthController(_authService);
            _response = new ApiResponse();
        }
        //  [Theory]
        // [InlineData(new LoginDto{ UserName = "Ali123",Password = "Ali&123" }, getUserDtoWithUserNotFound())]
        [Fact]
        public async Task Login_WithNotFondUser_Test()
        {
            //arr
            var login = new LoginDto { UserName = "Ali123",Password="Ali&123" };
            var response = getUserDtoWithUserNotFound();
            A.CallTo(()=> _authService.LoginAsync(login)).Returns(response);
            //act
            var actual= _authController.Login(login);
            
            //acc
            var acceptResult = "there is not email or password";
            var acceptIsuccess = false;
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(actual.Result);
            var ApiResponseResult = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal(acceptResult,ApiResponseResult.Message);
            Assert.Equal(acceptIsuccess, ApiResponseResult.IsSuccess);
        }
        [Fact]
        public async Task Login_WithUser_ReturnApiResponseWithUserDtoResult_Test()
        {
            //arr
            var login = new LoginDto { UserName = "Ali123", Password = "Ali&123" };
            var response = getUserDto();
            A.CallTo(() => _authService.LoginAsync(login)).Returns(response);
            //act
            var actual = _authController.Login(login);

            //acc
            var acceptIsuccess = true;
            //Assert
            var okResult = Assert.IsType<OkObjectResult>(actual.Result);
            var ApiResponseResult = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.IsType<UserDto>(ApiResponseResult.Result);
            Assert.Equal(acceptIsuccess, ApiResponseResult.IsSuccess);

        }
        [Fact]
        public async Task Register_WithFaildRegister_ApiResponseWithResultNull_Test()
        {
            //arrang

            var response = getUserDtoWithAlreadyExsit();
            var register = new RegisterDto() { UserName = "Ahmed123", Password = "Ahmed&123", PhoneNumber = "01128739", Name = "Ahmed" };
            A.CallTo(() => _authService.RegisterAsync(register)).Returns(response);

            //actual
            var registerResult = _authController.RegisterAsync(register);

            //accept

            var exceptMessa = "this user Name is already created";
            object exceptResult = null;
            //assert
            Assert.NotNull(registerResult);
            var result = Assert.IsType<OkObjectResult>(registerResult.Result);
            var apiResponse = Assert.IsType<ApiResponse>(result.Value);
            Assert.Equal(exceptMessa, apiResponse.Message);
            Assert.Equal(exceptResult, apiResponse.Result);
        }

        [Fact]
        public async Task Register_WithSuccessRegister_ApiResponseWithResultOfUserDto_Test()
        {
            //arrang

            var response = getUserDto();
            var register = new RegisterDto() { UserName = "Ahmed123", Password = "Ahmed&123",PhoneNumber="01128739",Name="Ahmed" };
            A.CallTo(() => _authService.RegisterAsync(register)).Returns(response);

            //actual
            var registerResult = _authController.RegisterAsync(register);

            //accept

            var exceptuser = "Ahmed123";
            Assert.NotNull(registerResult);
            var result = Assert.IsType<OkObjectResult>(registerResult.Result);
            var apiResponse = Assert.IsType<ApiResponse>(result.Value);
            var userDto=Assert.IsType<UserDto>(apiResponse.Result);
            Assert.Equal(exceptuser, userDto.UserName);
        }

        private ApiResponse getUserDto()
        {
            var user = new UserDto
            {
                UserName = "Ahmed123",
                Id = "ab60aafc-64ee-4db7-ab94-37c1dbf75633",
                ExpiresOn = DateTime.UtcNow,
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBbGkxMjMiLCJqdGkiOiJkNWI2NTNhMS0wMDg5LTQ5NmUtYjU0My0xMmEzYTMyYzJkYTIiLCJJZCI6ImFiNjBhYWZjLTY0ZWUtNGRiNy1hYjk0LTM3YzFkYmY3NTYzMyIsImV4cCI6MTcxMzIzNjI4MSwiaXNzIjoiU2VjdXJlQXBpIiwiYXVkIjoiU2VjdXJlQXBpVXNlciJ9.kOR3KIWtZ9W3IVPrjUkzqt3T-9YJtEF8bqglVPeroLQ",
                isAuthenticated = true,
            };
            _response.Result = user;
            return _response;
        }
        private ApiResponse getUserDtoWithUserNotFound()
        {

            return ApiResponse.ErrorException(HttpErrors.NotFound, "there is not email or password");
        }
        private ApiResponse getUserDtoWithAlreadyExsit()
        {

            return ApiResponse.ErrorException(HttpErrors.BadRequest, "this user Name is already created");
        }

    }
}
