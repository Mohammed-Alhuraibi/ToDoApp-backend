using System.Security.Claims;
using ToDo.Services;

public class UserOperationsMiddleware
{
    private readonly RequestDelegate _next;

    public UserOperationsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserTaskService userTaskService)
    {
        // Check if the request is for user operations endpoint
        if (context.Request.Path.StartsWithSegments("/api/UserOperations/tasks"))
        {
            // Extract the user ID from the JWT token
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null)
            {
                Console.WriteLine($"Can apply operations User ID: {userIdClaim}");
            }
            else
            {
                // If user ID claim not found, return unauthorized response
                Console.WriteLine("User ID claim not found.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        // If the request is not for user operations endpoint, proceed to the next middleware
        await _next(context);
    }
}
