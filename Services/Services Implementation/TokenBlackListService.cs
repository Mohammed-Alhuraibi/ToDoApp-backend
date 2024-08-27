using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ToDo.Services
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly List<string> _blacklistedTokens = new();

        public void BlacklistToken(string token)
        {
            _blacklistedTokens.Add(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.Contains(token);
        }

        public bool IsUserTokenBlacklisted(string email)
        {
            return _blacklistedTokens.Any(t => GetEmailFromToken(t) == email);
        }

        private string GetEmailFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ReadToken(token) as JwtSecurityToken;
            return principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}
