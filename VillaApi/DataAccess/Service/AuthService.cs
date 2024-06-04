using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using VillaApi.DataAccess.Helper;
using VillaApi.Model;
using VillaApi.Model.modelDto;
//using Microsoft.AspNetCore.Identity;


namespace VillaApi.DataAccess.Service
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly ApiResponse _response;

        public AuthService(UserManager<ApplicationUser> userManger,IOptions<JWT>jwt)
        {
           _userManager = userManger;
            _jwt = jwt.Value;
            _response = new ApiResponse();
        }
        private async Task<List<Claim>> getclaimroles(ApplicationUser user)
        {
            var _option = new List<Claim>();
            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
               // new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id.ToString())
            };
            var userclaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userclaims);
            var roleUser = await _userManager.GetRolesAsync(user);
            foreach (var userrole in roleUser)
            {
                var role = await _userManager.FindByNameAsync(userrole);
                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userrole));
                    var roleClaims = await _userManager.GetClaimsAsync(role);
                    foreach (var claim in roleClaims)
                    {
                        claims.Add(claim);
                    }
                }
            }
            return claims;
        }

        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));
            var claimsrole = getclaimroles(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
            audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;


        }
        public async Task<ApiResponse> LoginAsync(LoginDto Input)
        {

            var user = await _userManager.FindByNameAsync(Input.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                return ApiResponse.ErrorException(HttpErrors.BadRequest, "there is not email or password");
            
            }
            var userdto = new UserDto();

            var jwtSecurityToken = await CreateToken(user);
           // var roles = await _userManager.GetRolesAsync(user);
            userdto.UserName = user.UserName;
            userdto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            userdto.isAuthenticated = true;
            userdto.ExpiresOn = jwtSecurityToken.ValidTo;
            userdto.Id = user.Id.ToString();
           
            _response.Result = userdto;
            return _response;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterDto Input)
        {
            if (await _userManager.FindByNameAsync(Input.UserName) is not null)
            {
                return ApiResponse.ErrorException(HttpErrors.BadRequest, "this user Name is already created");
            }

            var user = new ApplicationUser
            {
                Name = Input.Name,
                PhoneNumber = Input.PhoneNumber,
                UserName = Input.UserName
            };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
                return ApiResponse.ErrorException(HttpErrors.InternalServerError, "Error in Create User");

                var jwtSecurityToken = await CreateToken(user);
                await _userManager.UpdateAsync(user);
                var userDto= new UserDto
                {
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    isAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    UserName = user.UserName,
                    Id = user.Id.ToString()

            };
                _response.Result = userDto;
                return _response;
          
        }

    }
}
