using System.Linq.Expressions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Models;
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


        services.AddScoped<IWritableCollaborationsProvider, WritableCollaborationsProvider>();
        services.AddScoped<IWritableFarmProvider, WritableFarmProvider>();
        services.AddScoped<IWritableInnoGotchiesProvider, WritableInnoGotchiProvider>();
        services.AddScoped<IWritableProfilesProvider, WritableProfileProvider>();
        services.AddScoped<IWritableFarmStatisticsProvider, WritableFarmStatisticsProvider>();

        services.AddScoped<IReadableAvatarProvider, ReadableAvatarProvider>();
        services.AddScoped<IReadableFarmProvider, ReadableFarmProvider>();
        services.AddScoped<IReadableFarmStatisticsProvider, ReadableFarmStatisticsProvider>();
        services.AddScoped<IReadableInnoGotchiComponentsProvider, ReadableInnoGotchiComponentsProvider>();
        services.AddScoped<IReadableInnoGotchiProvider, ReadableInnoGotchiProvider>();
        services.AddScoped<IReadableProfileProvider, ReadableProfileProvider>();
        services.AddScoped<IReadableCollaborationsProvider, ReadableCollaborationsProvider>();

        services.AddScoped<ICollaborationManager, CollaborationsManager>();
        services.AddScoped<IFarmManager, FarmManager>();
        services.AddScoped<IFarmStatisticsManager, FarmStatisticsManager>();
        services.AddScoped<IInnoGotchiActionsManager, InnoGotchiActionsManager>();
        services.AddScoped<IInnoGotchiComponentsManager, InnoGotchiComponentsManager>();
        services.AddScoped<IInnoGotchiManager, InnoGotchiManager>();
        services.AddScoped<IProfileAnonymousActionsManager, ProfileAnonymousActionsManager>();
        services.AddScoped<IProfileAuthorizedActionsManager, ProfileAuthorizedActionsManager>();
        services.AddScoped<ISearchProfilesManager, SearchProfilesManager>();

        services.AddScoped<ISearchProfilesService, SearchProfilesService>();
        services.AddScoped<IJwtTokenGenerationService, JwtTokenGenerationService>();
        services.AddScoped<ITimeService, TimeService>();
        services.AddScoped<IDefaultAvatarService, DefaultAvatarService>();
        services.AddScoped<ISortingService<InnoGotchiModel>, SortingServiceBase<InnoGotchiModel>>(x =>
        {
            var sortByProperties =
                new Dictionary<int, Expression<Func<InnoGotchiModel, object>>>
                {
                    { 0, model => model.HappinessDaysCount },
                    { 1, model => model.Age },
                    { 2, model => model.HungerLevel },
                    { 3, model => model.ThirstyLevel },
                    { 4, model => model.Name },
                    { 5, model => model.Farm.Name }
                };
            return new SortingServiceBase<InnoGotchiModel>(sortByProperties);
        });
        services.AddScoped<IInnoGotchiSignsUpdateService>(x =>
        {
            var timeService = x.GetRequiredService<ITimeService>();
            return new InnoGotchiSignsUpdateService(timeService,
                timeToIncreaseHungerLevel: TimeSpan.FromDays(1),
                timeToIncreaseThirstLevel: TimeSpan.FromDays(1),
                timeToIncreaseAge: TimeSpan.FromDays(7));
        });

        return services;
    }
}