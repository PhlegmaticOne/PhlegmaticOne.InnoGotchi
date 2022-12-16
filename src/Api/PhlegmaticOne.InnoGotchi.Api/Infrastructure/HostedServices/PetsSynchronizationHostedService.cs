using PhlegmaticOne.HostedServices;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.HostedServices;

public class PetsSynchronizationHostedService : CrontabScheduledHostedService
{
    public PetsSynchronizationHostedService(IServiceScopeFactory serviceScopeFactory,
        ILogger<PetsSynchronizationHostedService> logger) : base(serviceScopeFactory, logger) { }

    protected override string Schedule => "*/15 * * * *";
    protected override bool IsExecuteOnServerRestart => true;

    protected override async Task ProcessInScopeAsync(IServiceScope serviceScope, CancellationToken token)
    {
        var serviceProvider = serviceScope.ServiceProvider;
        var innoGotchiesSynchronizationProvider =
            serviceProvider.GetRequiredService<IInnoGotchiesSynchronizationProvider>();

        await innoGotchiesSynchronizationProvider.SynchronizeAllPetsAsync(token);
    }
}