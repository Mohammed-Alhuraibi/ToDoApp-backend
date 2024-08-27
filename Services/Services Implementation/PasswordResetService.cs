using System.Threading.Tasks;
using ToDo.Repositories.Interfaces;
using Models.DTO;

namespace ToDo.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IConfirmationService _confirmationService;

        public PasswordResetService(IUserRepository userRepository, IEmailService emailService, IConfirmationService confirmationService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _confirmationService = confirmationService;
        }

        public async Task<bool> RequestPasswordReset(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !user.IsConfirmed || user.IsLoggedIn)
            {
                return false; // User with given email not found, not confirmed, or logged in
            }

            if (!string.IsNullOrEmpty(user.ResetCode))
            {
                return false; // Reset code already exists for this user
            }

            // Generate a unique reset code
            var resetCode = CodeGenerator.GenerateCode();
            user.ResetCode = resetCode;
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            // Send the reset code to the user's email
            await _emailService.SendEmailAsync(user.Email, "Password Reset Code", $"Your password reset code is: {resetCode}");

            // Schedule confirmation code deletion after 20 seconds
            try
            {
                _ = _confirmationService.ScheduleConfirmationCodeDeletion(user.UserId);
                Console.WriteLine("Scheduled confirmation code deletion.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scheduling confirmation code deletion: {ex.Message}");
            }
            return true;
        }

        public async Task<bool> ResetPassword(PasswordResetDto resetDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(resetDto.Email);
            if (user == null || user.ResetCode != resetDto.ResetCode || string.IsNullOrEmpty(user.ResetCode) || !user.IsConfirmed)
            {
                return false; // Invalid email, reset code, reset code already used, or user is not confirmed
            }

            // Update the user's password
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetDto.NewPassword);
            user.ResetCode = null; // Clear the reset code to mark it as used
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResendResetCode(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null  || !user.IsConfirmed || user.IsLoggedIn)
            {
                return false; // User not found, user not confirmed, or logged in
            }

            // Generate a new reset code
            var resetCode = CodeGenerator.GenerateCode();
            user.ResetCode = resetCode;
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            // Send the new reset code to the user's email
            await _emailService.SendEmailAsync(user.Email, "Password Reset Code", $"Your new password reset code is: {resetCode}");
           
            // Schedule confirmation code deletion after 20 seconds
            try
            {
                _ = _confirmationService.ScheduleConfirmationCodeDeletion(user.UserId);
                Console.WriteLine("Scheduled confirmation code deletion.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scheduling confirmation code deletion: {ex.Message}");
            }

            return true;
        }
    }
}
