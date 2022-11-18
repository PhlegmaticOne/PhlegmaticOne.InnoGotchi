using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.InnoGotchi.Domain.InnoGotchiPolicies;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConverters;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Services.Managers;
using PhlegmaticOne.InnoGotchi.Services.Providers;
using PhlegmaticOne.InnoGotchi.Services.Services;

namespace PhlegmaticOne.InnoGotchi.Services.ServicesRegistration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserProfileValidator>();
        services.AddAutoMapper(builder =>
        {
            builder.AddMaps(typeof(FarmMapperConfiguration).Assembly);
        });
        services.AddTransient<AvatarToAvatarDataConverter>();

        services.AddScoped<IJwtTokenGenerationService, JwtTokenGenerationService>();

        services.AddScoped<IAvatarsProvider, AvatarsProvider>();
        services.AddScoped<IFarmProvider, FarmProvider>();
        services.AddScoped<IInnoGotchiComponentsProvider, InnoGotchiComponentsProvider>();
        services.AddScoped<IInnoGotchiesProvider, InnoGotchiProvider>();
        services.AddScoped<IProfilesProvider, ProfilesProvider>();

        services.AddScoped<IFarmManager, FarmManager>();
        services.AddScoped<IInnoGotchiComponentsManager, InnoGotchiComponentsManager>();
        services.AddScoped<IInnoGotchiesManager, InnoGotchiManager>();
        services.AddScoped<IProfileAnonymousActionsManager, ProfileAnonymousActionsManager>();
        services.AddScoped<IProfileAuthorizedActionsManager, ProfileAuthorizedActionsManager>();

        services.AddScoped<IInnoGotchiActionsPolicy>(_ =>
            new InnoGotchiActionsPolicy(TimeSpan.FromDays(1), TimeSpan.FromDays(1), TimeSpan.FromDays(1)));

        return services;
    }
}