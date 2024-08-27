using System.Threading.Tasks;
using ToDo.Models;
using System.Security.Claims;

namespace ToDo.Services
{
    public interface IAuthenticationService
    {
        Task<(string accessToken, string refreshToken)> AuthenticateUserAsync(string email, string password);
        Task LogoutUserAsync(string token);
        Task<(string NewAccessToken, string NewRefreshToken)> RefreshAccessTokenAsync(string refreshToken);
        ClaimsPrincipal GetPrincipalFromToken(string token);
        bool IsUserLoggedIn(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
    }
}
