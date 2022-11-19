using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConverters;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Services.Managers;
using PhlegmaticOne.InnoGotchi.Services.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Providers.Writable;
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


        services.AddScoped<IWritableFarmProvider, WritableFarmProvider>();
        services.AddScoped<IWritableInnoGotchiesProvider, WritableInnoGotchiProvider>();
        services.AddScoped<IWritableProfilesProvider, WritableProfileProvider>();
        services.AddScoped<IWritableFarmStatisticsProvider, WritableFarmStatisticsProvider>();

        services.AddScoped<IReadableAvatarProvider, ReadableAvatarProvider>();
        services.AddScoped<IReadableFarmProvider, ReadableFarmProvider>();
        services.AddScoped<IReadableInnoGotchiComponentsProvider, ReadableInnoGotchiComponentsProvider>();
        services.AddScoped<IReadableInnoGotchiProvider, ReadableInnoGotchiProvider>();
        services.AddScoped<IReadableProfileProvider, ReadableProfileProvider>();

        services.AddScoped<IFarmManager, FarmManager>();
        services.AddScoped<IInnoGotchiActionsManager, InnoGotchiActionsManager>();
        services.AddScoped<IInnoGotchiComponentsManager, InnoGotchiComponentsManager>();
        services.AddScoped<IInnoGotchiManager, InnoGotchiManager>();
        services.AddScoped<IProfileAnonymousActionsManager, ProfileAnonymousActionsManager>();
        services.AddScoped<IProfileAuthorizedActionsManager, ProfileAuthorizedActionsManager>();

        services.AddScoped<IJwtTokenGenerationService, JwtTokenGenerationService>();
        services.AddScoped<ITimeService, TimeService>();
        services.AddScoped<IInnoGotchiSignsUpdateService>(x =>
        {
            var timeService = x.GetRequiredService<ITimeService>();
            return new InnoGotchiSignsUpdateService(timeService,
                timeToIncreaseHungerLevel: TimeSpan.FromDays(1),
                timeToIncreaseThirstLevel: TimeSpan.FromDays(1),
                timeToIncreaseAge: TimeSpan.FromMinutes(20));
        });

        return services;
    }
}