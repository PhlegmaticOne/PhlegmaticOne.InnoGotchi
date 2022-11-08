namespace PhlegmaticOne.InnoGotchi.Web.Middlewares;

public static class CustomAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder applicationBuilder) => 
        applicationBuilder.UseMiddleware<CustomAuthenticationMiddleware>();
}