using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViFlix.Core.Services.RefreshTokens;
using ViFlix.Core.ViewModels.RefreshToken;
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

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestViewModel request)
        {
            var refreshToken =  _context.RefreshToken.FirstOrDefault(x => x.Token == request.RefreshToken);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                return Unauthorized("Invalid refresh token.");
            }

            
            if (refreshToken.Expires < DateTime.UtcNow)
            {
                return Unauthorized("Refresh token has expired.");
            }

            
            var jwtToken = GenerateJwtToken(refreshToken.UserId);

            
            var newRefreshToken = GenerateRefreshToken(refreshToken.UserId);

            
            refreshToken.Revoked = DateTime.UtcNow;
            _context.RefreshToken.Add(newRefreshToken);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = jwtToken,
                refreshToken = newRefreshToken.Token
            });
        }

        #region Helper Methods

        private string GenerateJwtToken(long userId)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
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
    }
}
