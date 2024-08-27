
public static class UserOperationsMiddlewareExtensions
{
    public static IApplicationBuilder UseUserOperationsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserOperationsMiddleware>();
    }
}
