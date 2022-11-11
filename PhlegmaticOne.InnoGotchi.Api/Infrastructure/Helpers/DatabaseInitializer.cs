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

        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync();
        }
        else
        {
            await dbContext.Database.EnsureCreatedAsync();
        }

        var set = dbContext.Set<InnoGotchiComponent>();

        if (set.Any())
        {
            return;
        }

        var componentFiles = WwwRootHelper.GetComponents(webHostEnvironment);

        foreach (var component in componentFiles)
        {
            foreach (var componentImageUrl in component.Value)
            {
                set.Add(new()
                {
                    Name = component.Key,
                    ImageUrl = componentImageUrl
                });
            }
        }

        var userProfiles = dbContext.Set<UserProfile>();

        userProfiles.Add(new UserProfile
        {
            User = new User
            {
                Email = "test@gmail.com",
                Password = passwordHasher.Hash("Qwerty_1234")
            },
            FirstName = "Firstname",
            SecondName = "Secondname",
            JoinDate = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }
}