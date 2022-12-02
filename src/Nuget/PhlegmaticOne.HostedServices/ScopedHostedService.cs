using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhlegmaticOne.HostedServices.Base;

namespace PhlegmaticOne.HostedServices;

public abstract class ScopedHostedService : HostedServiceBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly ILogger Logger;

    protected ScopedHostedService(IServiceScopeFactory serviceScopeFactory, ILogger logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        Logger = logger;
    }

    protected override async Task ProcessAsync(CancellationToken token)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await ProcessInScopeAsync(scope, token);
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, GetType().Name);
            token.ThrowIfCancellationRequested();
        }
    }

    protected abstract Task ProcessInScopeAsync(IServiceScope serviceScope, CancellationToken token);
}