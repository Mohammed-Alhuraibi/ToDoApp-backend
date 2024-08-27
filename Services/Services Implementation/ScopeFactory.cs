public class ScopeFactory : IScopeFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ScopeFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }
}