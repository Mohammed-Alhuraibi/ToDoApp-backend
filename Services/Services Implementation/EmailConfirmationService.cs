using System;
using System.Threading.Tasks;
using ToDo.Repositories.Interfaces;
using ToDo.Services.Interfaces;
using Models.DTO;
using ToDo.Models;

namespace ToDo.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IConfirmationService _confirmationService;

        public EmailConfirmationService(IUserRepository userRepository, IEmailService emailService, IConfirmationService confirmationService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _confirmationService = confirmationService;
        }

        public async Task<(bool Success, string Message)> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(confirmEmailDto.Email);
            if (user == null)
            {
                return (false, "User not found.");
            }

            if (user.IsConfirmed)
            {
                return (false, "Email is already confirmed.");
            }

            if (user.ConfirmationCode != confirmEmailDto.Code)
            {
                return (false, "Invalid confirmation code.");
            }

            user.IsConfirmed = true;
            user.ConfirmationCode = null; // Clear the confirmation code
            await _userRepository.SaveChangesAsync();

            return (true, "Email confirmed successfully.");
        }

        public async Task<(bool Success, string Message)> ResendConfirmationCodeAsync(ResendConfirmationCodeDto resendConfirmationCodeDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(resendConfirmationCodeDto.Email);
            if (user == null)
            {
                return (false, "User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(resendConfirmationCodeDto.Password, user.Password))
            {
                return (false, "Invalid password.");
            }

            if (user.IsConfirmed)
            {
                return (false, "Email is already confirmed.");
            }

            var confirmationCode = CodeGenerator.GenerateCode();
            user.ConfirmationCode = confirmationCode;
            await _userRepository.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Your new confirmation code is: {confirmationCode}");

            try
            {
                _ = _confirmationService.ScheduleConfirmationCodeDeletion(user.UserId);
                Console.WriteLine("Scheduled confirmation code deletion.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scheduling confirmation code deletion: {ex.Message}");
            }

            return (true, "Confirmation code sent successfully.");
        }
    }
}
