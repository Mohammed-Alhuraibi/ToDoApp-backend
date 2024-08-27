using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ToDo.Models;
using ToDo.Repositories.Interfaces;

namespace ToDo.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public AuthenticationService(IConfiguration config, IUserRepository userRepository, ITokenBlacklistService tokenBlacklistService)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenBlacklistService = tokenBlacklistService ?? throw new ArgumentNullException(nameof(tokenBlacklistService));
        }

        public async Task<(string accessToken, string refreshToken)> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.IsConfirmed)
                throw new UnauthorizedAccessException("Email not confirmed. Please check your email to confirm your account.");

            if (user.IsLoggedIn)
                throw new InvalidOperationException("User is already logged in.");

            user.IsLoggedIn = true;
            await _userRepository.SaveChangesAsync();

            var accessToken = GenerateJwtToken(user, TimeSpan.FromMinutes(1));
            var refreshToken = GenerateJwtToken(user, TimeSpan.FromDays(1));

            return (accessToken, refreshToken);
        }

        public async Task LogoutUserAsync(string token)
        {
            var principal = GetPrincipalFromToken(token);
            if (principal == null)
                throw new InvalidOperationException("Invalid token.");

            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    if (!user.IsLoggedIn)
                        throw new InvalidOperationException("User is already logged out.");

                    user.IsLoggedIn = false;
                    await _userRepository.SaveChangesAsync();
                }
            }

            _tokenBlacklistService.BlacklistToken(token);
        }

        public async Task<(string NewAccessToken, string NewRefreshToken)> RefreshAccessTokenAsync(string refreshToken)
        {
            var principal = GetPrincipalFromToken(refreshToken);
            if (principal == null)
                throw new UnauthorizedAccessException("Invalid token");

            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null)
                throw new UnauthorizedAccessException("Invalid token claims");

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            return (GenerateJwtToken(user, TimeSpan.FromMinutes(1)), GenerateJwtToken(user, TimeSpan.FromDays(1)));
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Secret"])),
                ValidateLifetime = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GenerateJwtToken(User user, TimeSpan expiresIn)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.Add(expiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool IsUserLoggedIn(string email, string password)
        {
            var user = _userRepository.GetUserByEmailAsync(email).Result;
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return false;

            return user.IsLoggedIn;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        ClaimsPrincipal IAuthenticationService.GetPrincipalFromToken(string token)
        {
            return GetPrincipalFromToken(token);
        }
    }
}
