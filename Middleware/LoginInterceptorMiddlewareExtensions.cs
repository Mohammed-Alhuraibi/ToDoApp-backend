using Microsoft.AspNetCore.Builder;

public static class LoginInterceptorMiddlewareExtensions
{
    public static IApplicationBuilder UseLoginInterceptor(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoginInterceptorMiddleware>();
    }
}
