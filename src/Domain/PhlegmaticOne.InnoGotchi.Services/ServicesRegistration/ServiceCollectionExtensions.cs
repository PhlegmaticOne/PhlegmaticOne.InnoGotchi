using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Services.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Services;

namespace PhlegmaticOne.InnoGotchi.Services.ServicesRegistration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterProfileValidator>();
        services.AddAutoMapper(builder => { builder.AddMaps(typeof(FarmMapperConfiguration).Assembly); });
        services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);

        services.AddScoped<IWritableCollaborationsProvider, WritableCollaborationsProvider>();
        services.AddScoped<IWritableFarmProvider, WritableFarmProvider>();
        services.AddScoped<IWritableInnoGotchiesProvider, WritableInnoGotchiProvider>();
        services.AddScoped<IWritableProfilesProvider, WritableProfileProvider>();
        services.AddScoped<IWritableFarmStatisticsProvider, WritableFarmStatisticsProvider>();
        services.AddScoped<IInnoGotchiesSynchronizationProvider, InnoGotchiesSynchronizationProvider>();

        services.AddScoped<IReadableInnoGotchiProvider, ReadableInnoGotchiProvider>();
        services.AddScoped<IInnoGotchiOwnChecker, InnoGotchiOwnChecker>();

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
                TimeSpan.FromDays(1),
                TimeSpan.FromDays(1),
                TimeSpan.FromDays(7));
        });

        return services;
    }
}