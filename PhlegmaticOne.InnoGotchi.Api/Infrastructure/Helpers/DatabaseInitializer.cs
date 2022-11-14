using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models;
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

        await CreateOrMigrate(dbContext);

        var set = dbContext.Set<InnoGotchiComponent>();

        if (set.Any())
        {
            return;
        }

        await set.AddRangeAsync(SeedComponents(webHostEnvironment));
        await dbContext.Set<UserProfile>().AddAsync(SeedUserProfile(passwordHasher));
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

    private static IEnumerable<InnoGotchiComponent> SeedComponents(IWebHostEnvironment webHostEnvironment)
    {
        var componentFiles = WwwRootHelper.GetComponents(webHostEnvironment);

        foreach (var component in componentFiles)
        {
            foreach (var componentImageUrl in component.Value)
            {
                yield return new()
                {
                    Name = component.Key,
                    ImageUrl = componentImageUrl
                };
            }
        }
    }

    private static UserProfile SeedUserProfile(IPasswordHasher passwordHasher) =>
        new()
        {
            User = new User
            {
                Email = "test@gmail.com",
                Password = passwordHasher.Hash("Qwerty_1234")
            },
            FirstName = "Firstname",
            LastName = "Secondname",
            JoinDate = DateTime.UtcNow
        };
}