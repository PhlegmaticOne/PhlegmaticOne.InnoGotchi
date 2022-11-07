using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.PasswordHasher.Extensions;

public static class PasswordHasherExtensions
{
    public static IServiceCollection AddPasswordHasher(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IPasswordHasher>(_ => new SecurePasswordHasher());
        return serviceCollection;
    }
}