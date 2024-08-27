using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDo.Services.Interfaces;
using Models.DTO;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ConfirmController : ControllerBase
    {
        private readonly IEmailConfirmationService _emailConfirmationService;

        public ConfirmController(IEmailConfirmationService emailConfirmationService)
        {
            _emailConfirmationService = emailConfirmationService;
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            var result = await _emailConfirmationService.ConfirmEmailAsync(confirmEmailDto);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("resend")]
        public async Task<IActionResult> ResendConfirmationCode([FromBody] ResendConfirmationCodeDto resendConfirmationCodeDto)
        {
            var result = await _emailConfirmationService.ResendConfirmationCodeAsync(resendConfirmationCodeDto);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
