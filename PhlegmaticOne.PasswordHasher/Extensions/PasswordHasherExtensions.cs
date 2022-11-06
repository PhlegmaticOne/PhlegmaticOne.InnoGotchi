using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.PasswordHasher.Extensions;

public static class PasswordHasherExtensions
{
    public static IServiceCollection AddPasswordHasher(this IServiceCollection serviceCollection, int hashIterations = 100)
    {
        serviceCollection
            .AddSingleton<IPasswordHasher>(x => new SecurePasswordHasher(hashIterations));
        return serviceCollection;
    }
}