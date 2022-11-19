using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var webHostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var serverAddressProvider = scope.ServiceProvider.GetRequiredService<IServerAddressProvider>();
        var timeService = scope.ServiceProvider.GetRequiredService<ITimeService>();

        await CreateOrMigrate(dbContext);

        var innoGotchiesSet = dbContext.Set<InnoGotchiComponent>();
        var userProfilesSet = dbContext.Set<UserProfile>();

        if (innoGotchiesSet.Any() == false)
        {
            await innoGotchiesSet.AddRangeAsync(SeedComponents(webHostEnvironment, serverAddressProvider));
        }

        if (userProfilesSet.Any() == false)
        {
            await userProfilesSet.AddAsync(SeedUserProfile(passwordHasher, timeService));
        }

        await dbContext.SaveChangesAsync();
    }

    private static async Task CreateOrMigrate(ApplicationDbContext dbContext)
    {
        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync();
        }
        else
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
    }

    private static IEnumerable<InnoGotchiComponent> SeedComponents(IWebHostEnvironment webHostEnvironment, IServerAddressProvider serverAddressProvider)
    {
        var componentFiles = WwwRootHelper.GetComponents(webHostEnvironment);
        var serverAddress = serverAddressProvider.ServerAddressUri;
        foreach (var component in componentFiles)
        {
            foreach (var componentImageUrl in component.Value)
            {
                yield return new()
                {
                    Name = component.Key,
                    ImageUrl = serverAddress.Combine(componentImageUrl).AbsoluteUri
                };
            }
        }
    }

    private static UserProfile SeedUserProfile(IPasswordHasher passwordHasher, ITimeService timeService) =>
        new()
        {
            User = new User
            {
                Email = "test@gmail.com",
                Password = passwordHasher.Hash("Qwerty_1234")
            },
            Avatar = new Avatar(),
            FirstName = "Firstname",
            LastName = "Secondname",
            JoinDate = timeService.Now(),
            Farm = new Farm
            {
                FarmStatistics = new FarmStatistics
                {
                    LastDrinkTime = timeService.Now(),
                    LastFeedTime = timeService.Now()
                },
                Name = "my farm"
            }
        };
}