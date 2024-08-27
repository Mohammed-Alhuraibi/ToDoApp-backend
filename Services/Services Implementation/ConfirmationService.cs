using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using ToDo.Repositories.Interfaces;

public class ConfirmationService : IConfirmationService
{
    private readonly IScopeFactory _scopeFactory;

    public ConfirmationService(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    public async Task ScheduleConfirmationCodeDeletion(int userId, int delayInSeconds = 120)
    {
        Console.WriteLine($"Scheduling confirmation code deletion at {DateTime.Now} for user {userId}...");
        await Task.Delay(delayInSeconds * 1000);
        Console.WriteLine($"Deleting confirmation code at {DateTime.Now} for user {userId}...");

        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                Console.WriteLine("Created new scope for service provider...");
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                Console.WriteLine("Resolved user repository from scoped service provider...");

                Console.WriteLine($"Getting user with ID {userId}...");
                var user = await userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    Console.WriteLine($"User found. {user.ConfirmationCode}");
                    user.ConfirmationCode = null;
                    user.ResetCode = null;
                    await userRepository.SaveChangesAsync();
                    Console.WriteLine("Confirmation code deleted and changes saved.");
                }
                else
                {
                    Console.WriteLine("User not found...");
                }
            }
            Console.WriteLine("Scope disposed after operation...");
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            Console.WriteLine($"Error deleting confirmation code: {ex.Message}");
        }
    }
}
