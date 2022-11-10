using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.DataService.Implementation;
using PhlegmaticOne.DataService.Interfaces;

namespace PhlegmaticOne.DataService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataService<TContext>(this IServiceCollection serviceCollection)
        where TContext : DbContext
    {
        serviceCollection.AddScoped<IDataService>(x =>
        {
            var dbContext = x.GetRequiredService<TContext>();
            return new DbContextDataService(dbContext);
        });
        return serviceCollection;
    }
}