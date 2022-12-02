using PhlegmaticOne.InnoGotchi.Api.Infrastructure.HostedServices;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Services.Synchronization;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPetsSynchronizationHostedService(
        this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
    {
        var shouldSynchronize = true;
        if (webHostEnvironment.IsDevelopment() == false)
        {
            shouldSynchronize = false;
            services.AddHostedService<PetsSynchronizationHostedService>();
        }

        services.AddSingleton<IShouldSynchronizePetsProvider, ShouldSynchronizePetsProvider>(
            _ => new ShouldSynchronizePetsProvider(shouldSynchronize));

        return services;
    }
}