using FirstShop.Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViFlix.Core.Services.RefreshTokens;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Core.ViewModels.RefreshToken;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Context;
using ViFlix.Data.RefreshTokens;

namespace ViFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IUserService _userServices;

        public AuthController(IConfiguration config, AppDbContext context , IUserService userServices)
        {
            _config = config;
            _context = context;
            _userServices = userServices;
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestViewModel request)
        {
            var refreshToken = _context.RefreshToken
                .FirstOrDefault(x => x.Token == request.RefreshToken);

            if (refreshToken == null || !refreshToken.IsActive)
                return Unauthorized("Invalid or revoked refresh token.");

            if (refreshToken.Expires < DateTime.UtcNow)
                return Unauthorized("Refresh token has expired.");

            
            var user =  _context.Users.FirstOrDefault(u => u.Id == refreshToken.UserId);
            if (user == null)
                return Unauthorized("User not found.");

            
            var jwtToken = GenerateJwtToken(
                refreshToken.UserId,
                user.UserName,
                user.Email
            );

            

            return Ok(new
            {
                token = jwtToken,
                refreshToken = refreshToken.Token
            });
        }


        #region Helper Methods

        private string GenerateJwtToken(long userId, string username, string email)
        {
            var jwtKey = _config["JwtSettings:Key"];
            var jwtIssuer = _config["JwtSettings:Issuer"];
            var jwtAudience = _config["JwtSettings:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email ?? "")
        };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Data.RefreshTokens.RefreshToken GenerateRefreshToken(long userId)
        {
            return new Data.RefreshTokens.RefreshToken
            {
                Token = Guid.NewGuid().ToString("N"),
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(7), 
                Created = DateTime.UtcNow
            };
        }

        #endregion

        [HttpPost("LoginJWT")]
        public IActionResult LoginJWT([FromQuery] LoginViewModel login)
        {
            string pass = PasswordHelper.EncodePasswordMd5(login.Password);
            if (login.UserName != null && pass != null)
            {
                var user = _userServices.Login(login);
                if (user == null) return Unauthorized("User not found");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Name ,login.UserName),
                new Claim(ClaimTypes.Email , user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Issuer = _config["JwtSettings:Issuer"],
                    Audience = _config["JwtSettings:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                
                var refreshToken = new RefreshToken
                {
                    Token = Guid.NewGuid().ToString("N"),
                    UserId = user.Id,
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow
                };

                _context.RefreshToken.Add(refreshToken);
                _context.SaveChanges(); 

                return Ok(new
                {
                    Token = tokenString,
                    RefreshToken = refreshToken.Token
                });
            }

            return Unauthorized("Username and password don't match!");
        }
    }
}
