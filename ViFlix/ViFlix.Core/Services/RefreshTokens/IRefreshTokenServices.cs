using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.RefreshTokens;

namespace ViFlix.Core.Services.RefreshTokens
{
    public interface IRefreshTokenServices
    {
        Task<string> GenerateRefreshTokenAsync(long userId);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task InvalidateRefreshTokenAsync(string token);
        Task<bool> IsRefreshTokenValidAsync(string token);
    }
}
