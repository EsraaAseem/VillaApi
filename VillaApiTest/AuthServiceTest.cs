using FakeItEasy;
using VillaApi.Model.modelDto;
using VillaApi.Model;
using Microsoft.AspNetCore.Identity;
using VillaApi.DataAccess.Service;
using Microsoft.Extensions.Options;
using DnsClient;
using VillaApi.DataAccess.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace VillaApiTest
{
    public class AuthServiceTest
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions< JWT> _jwt;
        private readonly LoginDto _loginDto;
        private readonly RegisterDto _registerDto;
        private readonly ApplicationUser _user;
        private readonly ApiResponse _response;

        public AuthServiceTest()
        {
           var option=new JWT { Key = "sz8eI7OdHBrjrIo8j9nTW/rQyO1OvY0pAQ2wDKQZw/0=", Issuer = "SecureApi", Audience = "SecureApiUser", DurationInDays = 30 };

          var jwtOptions = Options.Create(option);
            _userManager = A.Fake<UserManager<ApplicationUser>>();
           _jwt=A.Fake<IOptions<JWT>>();
            
            _authService = new AuthService(_userManager,jwtOptions);
            _user=new ApplicationUser();
            _loginDto= new LoginDto { UserName = "Ali123", Password = "Ali&123" };
            _registerDto= new RegisterDto() { UserName = "Ahmed123", Password = "Ahmed&123", PhoneNumber = "01128739", Name = "Ahmed" };
            _response=new();  
        }

        [Fact]
        public async Task LoginService_userNotFound_ReturnApiResponseWithNotFondUserMessage_Test()
        {
            //Arr
            A.CallTo( () => _userManager.FindByNameAsync(_loginDto.UserName)).Returns((ApplicationUser)null);
            //actual
            var res = await _authService.LoginAsync(_loginDto);
            //acc
            var userMessage = "there is not email or password";
            var status = HttpErrors.BadRequest;

            Assert.NotNull(res);
           Assert.IsType<ApiResponse>(res);

             Assert.Equal(userMessage, res.Message);
             Assert.Equal(status,res.Status);

        }

        [Fact]
        public async Task LoginService_PasswordWrong_ApiResponseWithBadRequest_Test()
        {
            //arrang
            var user = getUser();
            A.CallTo(() => _userManager.FindByNameAsync(_loginDto.UserName)).Returns(user);

            A.CallTo(() => _userManager.CheckPasswordAsync(user,_loginDto.Password)).Returns(false);
            //actual
            var res = await _authService.LoginAsync(_loginDto);
            //acc
            var userMessage = "there is not email or password";
            var status = HttpErrors.BadRequest;

            Assert.NotNull(res);
            Assert.IsType<ApiResponse>(res);

            Assert.Equal(userMessage, res.Message);
            Assert.Equal(status, res.Status);

        }
        [Fact]
        public async Task Login_service_userDto_UserExsit_Test()
        {
            //arrang
            var user = getUser();

            A.CallTo(() => _userManager.FindByNameAsync(_loginDto.UserName)).Returns(user);

            A.CallTo(() => _userManager.CheckPasswordAsync(user, _loginDto.Password)).Returns(true);

            A.CallTo(() => _userManager.GetRolesAsync(A<ApplicationUser>.Ignored)).Returns(new List<string> { "Passenger" });
            //actual
            var res =await _authService.LoginAsync(_loginDto);
            //acc
            var userNameResult ="Ahmed123";
            var isAuthenticatedResult = true;
            //Assert
            Assert.NotNull(res);
           var apiResponse= Assert.IsType<ApiResponse>(res);
            var userDto=Assert.IsType<UserDto>(apiResponse.Result);
            Assert.Equal(userNameResult,userDto.UserName);
            Assert.Equal(isAuthenticatedResult, res.IsSuccess);

        }
        [Fact]
        public async Task Register_UserAlreadyExsit_ReturnAPiResponseWithBadRequest_Test()
        {
            //arr
            var user = getUser();
            var register = new RegisterDto() { UserName = "Ahmed123", Password = "Ahmed&123", PhoneNumber = "01128739", Name = "Ahmed" };
            A.CallTo(() => _userManager.FindByNameAsync(register.UserName)).Returns(user);
            //actual
            var res = await _authService.RegisterAsync(register);
            //Accept
            object result = null;
            var message = "this user Name is already created";
            Assert.NotNull(res);
            var apiResponse=Assert.IsType<ApiResponse>(res);    
            Assert.Equal(message,apiResponse.Message);
            Assert.Equal(result,apiResponse.Result);
        }
        [Fact]
        public async Task Register_FailCreateUser_ReturnAPiResponseWithBadRequest_Test()
        {
            //arr
            var user = getUser();
            var register = new RegisterDto() { UserName = "Ahmed123", Password = "Ahmed&123", PhoneNumber = "01128739", Name = "Ahmed" };
            A.CallTo(() => _userManager.FindByNameAsync(register.UserName)).Returns((ApplicationUser)null);
            A.CallTo(() => _userManager.CreateAsync(A<ApplicationUser>._, register.Password)).Returns(Task.FromResult(IdentityResult.Failed()));

            //actual
            var res = await _authService.RegisterAsync(register);
            //Accept
            object result = null;
            var message = "Error in Create User";
            var status = HttpErrors.InternalServerError;
            Assert.NotNull(res);
            var apiResponse = Assert.IsType<ApiResponse>(res);
            Assert.Equal(message, apiResponse.Message);
            Assert.Equal(result, apiResponse.Result);
            Assert.Equal(status, apiResponse.Status);
        }

        [Fact]
        public async Task Register_service_userDto_Test()
        {
            var user = getUser();
            var register = new RegisterDto() { UserName = "Ahmed123", Password = "Ahmed&123", PhoneNumber = "01128739", Name = "Ahmed" };
            A.CallTo(() => _userManager.FindByNameAsync(register.UserName)).Returns((ApplicationUser)null);
              A.CallTo(() => _userManager.CreateAsync(A<ApplicationUser>._, register.Password)).Returns(Task.FromResult(IdentityResult.Success));

            //actual
            var res = await _authService.RegisterAsync(register);
            //Accept
            var userName ="Ahmed123";
            //assert
            Assert.NotNull(res);
            var apiResponse = Assert.IsType<ApiResponse>(res);
            var userDto = Assert.IsType<UserDto>(apiResponse.Result);
            Assert.Equal(userName,userDto.UserName);
        }
        private ApplicationUser getUser()
        {
            var user = new ApplicationUser
            {
                Id=Guid.Parse ("851e7e1a-463c-4b1c-a8f1-fb86b61ea68a"),
                UserName = "Ahmed123",
                PhoneNumber= "01128739",
                Name= "Ahmed",
                PasswordHash= "AQAAAAIAAYagAAAAEJXbQr4LIxibi7Dvd1vlKQsKnIEodfFU7ZmlKsdD3tdXYtbnGlLmxTnT9yb/ZOzSvQ==",
                SecurityStamp= "2MO6SDEZ4UGRVUXOTRV6H6N2GWYSSGGW",
                ConcurrencyStamp= "e7405740-3bbd-444e-ac8d-b519820c8ec5",

            };
            return user;
        }
        private ApiResponse getUserDto()
        {
            var user = new UserDto
            {
                UserName = "Ahmed123",
                Id = "ab60aafc-64ee-4db7-ab94-37c1dbf75633",
               /* ExpiresOn = DateTime.UtcNow,
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBbGkxMjMiLCJqdGkiOiJkNWI2NTNhMS0wMDg5LTQ5NmUtYjU0My0xMmEzYTMyYzJkYTIiLCJJZCI6ImFiNjBhYWZjLTY0ZWUtNGRiNy1hYjk0LTM3YzFkYmY3NTYzMyIsImV4cCI6MTcxMzIzNjI4MSwiaXNzIjoiU2VjdXJlQXBpIiwiYXVkIjoiU2VjdXJlQXBpVXNlciJ9.kOR3KIWtZ9W3IVPrjUkzqt3T-9YJtEF8bqglVPeroLQ",
               */ isAuthenticated = true,
            };
            _response.Result = user;
            return _response;
        }
        /*
            MethodInfo methodInfo = typeof(AuthService).GetMethod("CreateToken", BindingFlags.NonPublic | BindingFlags.Instance);
            var jwtSecurityTokenTask = (Task<JwtSecurityToken>)methodInfo.Invoke(_authService,parameters);
         */
    }
}
