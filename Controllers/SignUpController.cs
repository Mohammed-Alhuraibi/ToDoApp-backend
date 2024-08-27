using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDo.Services;
using Models.DTO;
using ToDo.Repositories.Interfaces;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public SignUpController(IRegistrationService registrationService, IEmailService emailService, IUserRepository userRepository)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            try
            {
                var user = await _registrationService.RegisterUser(userDto); // Modify this line
                await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Your confirmation code is: {user.ConfirmationCode}");

                // Log registration success
                Console.WriteLine("User registered successfully. Starting confirmation timeout.");

                return Ok(new { message = "User registered successfully. Please check your email for the confirmation code." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
