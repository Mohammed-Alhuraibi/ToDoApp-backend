using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDo.Services;
using Models.DTO;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;

        public ResetPasswordController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] EmailDto emailDto)
        {
            var result = await _passwordResetService.RequestPasswordReset(emailDto.Email);
            if (!result)
            {
                return BadRequest(new { message = "Unable to process password reset request. Make sure the email is correct, the account is confirmed, the user is not logged in, and a reset code has not already been requested." });
            }

            return Ok(new { message = "Password reset code sent. Please check your email." });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto resetDto)
        {
            var result = await _passwordResetService.ResetPassword(resetDto);
            if (!result)
            {
                return BadRequest(new { message = "Unable to reset password. Invalid email, reset code, reset code already used, or the user is not confirmed." });
            }

            return Ok(new { message = "Password reset successful." });
        }

        [HttpPost("resend")]
        public async Task<IActionResult> ResendResetCode([FromBody] EmailDto emailDto)
        {
            var result = await _passwordResetService.ResendResetCode(emailDto.Email);
            if (!result)
            {
                return BadRequest(new { message = "Unable to resend reset code. Make sure the email is correct, a reset code was requested, the account is confirmed, and the user is not logged in." });
            }

            return Ok(new { message = "Password reset code resent. Please check your email." });
        }
    }
}
