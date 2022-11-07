namespace PhlegmaticOne.InnoGotchi.Api.Services;

public static class JwtTokenGeneratorExtensions
{
    public static IServiceCollection AddJwtTokenGeneration(this IServiceCollection serviceCollection, TimeSpan expirationTime)
    {
        serviceCollection.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>(
            _ => new JwtTokenGenerator(expirationTime));
        return serviceCollection;
    }
}