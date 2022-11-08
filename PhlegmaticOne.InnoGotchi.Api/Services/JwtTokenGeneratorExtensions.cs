namespace PhlegmaticOne.InnoGotchi.Api.Services;

public static class JwtTokenGeneratorExtensions
{
    public static IServiceCollection AddJwtTokenGeneration(this IServiceCollection serviceCollection, IJwtOptions jwtOptions)
    {
        serviceCollection.AddTransient<IJwtTokenGenerator>(_ => new JwtTokenGenerator(jwtOptions));
        return serviceCollection;
    }
}