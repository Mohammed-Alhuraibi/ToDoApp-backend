using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;

public class LoginInterceptorMiddleware
{
    private readonly RequestDelegate _next;

    public LoginInterceptorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");

        // Process the request and wait for the response
        await _next(context);

        Console.WriteLine($"Response: {context.Response.StatusCode}");
        if (context.Request.Path.StartsWithSegments("/api/Login"))
        {

                // Log user information after the request is handled
                if (context.Response.StatusCode == StatusCodes.Status200OK)
                {
                    // Extract the user ID from the JWT token
                    var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim != null)
                    {
                        Console.WriteLine($"User ID claim: {userIdClaim.Value}");
                        Console.WriteLine($"User with ID {userIdClaim.Value} has logged in.");
                    }
                    else
                    {
                        Console.WriteLine("No user ID claim found.");
                    }
                }
        }

    }

    
}
