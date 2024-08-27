using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using System;
using System.Threading.Tasks;
using ToDo.Services;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public LoginController(IAuthenticationService authenticationService, ITokenBlacklistService tokenBlacklistService)
        {
            _authenticationService = authenticationService;
            _tokenBlacklistService = tokenBlacklistService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var (accessToken, refreshToken) = await _authenticationService.AuthenticateUserAsync(loginDto.Email, loginDto.Password);
                var user = await _authenticationService.GetUserByEmailAsync(loginDto.Email);

                var loginResponse = new LoginResponseDto
                {
                    UserName = user.UserName,
                    Token = accessToken,
                    RefreshToken = refreshToken,
                };
                Console.WriteLine("Login successed bro");

                return Ok(loginResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshToken)
        {
            try
            {
                var (newAccessToken, newRefreshToken) = await _authenticationService.RefreshAccessTokenAsync(refreshToken.RefreshToken);

                return Ok(new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] AccessTokenDto accessToken)
        {
            try
            {
                await _authenticationService.LogoutUserAsync(accessToken.AccessToken);
                return Ok(new { message = "Logout successful." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while logging out." });
            }
        }
        // [HttpGet("test")]
        // public async Task<IActionResult> Test(){
        //      return Ok(new { message = "Testing is good bro don't worry." });
        // }
    }
}
