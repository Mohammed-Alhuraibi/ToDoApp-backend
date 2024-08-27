using System;
using System.Threading.Tasks;
using ToDo.Models;
using ToDo.Repositories.Interfaces;
using Models.DTO;
using ToDo.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IConfirmationService _confirmationService;

    public RegistrationService(IUserRepository userRepository, IEmailService emailService, IConfirmationService confirmationService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
    }

    public async Task<User> RegisterUser(UserRegistrationDto userDto)
    {
        // Check if the email already exists
        if (await _userRepository.GetUserByEmailAsync(userDto.Email) != null)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        // Hash the password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        // Create a new user
        var user = new User
        {
            Email = userDto.Email,
            Password = hashedPassword,
            UserName = userDto.UserName,
            IsConfirmed = false,
            ConfirmationCode = CodeGenerator.GenerateCode()
        };

        await _userRepository.AddUserAsync(user);
        await _userRepository.SaveChangesAsync();
        
        // Log before scheduling
        Console.WriteLine("About to schedule confirmation code deletion...");

        // Schedule confirmation code deletion after 30 seconds
        try
        {
            _ = _confirmationService.ScheduleConfirmationCodeDeletion(user.UserId);
            Console.WriteLine("Scheduled confirmation code deletion.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error scheduling confirmation code deletion: {ex.Message}");
        }

        return user;
    }

}
