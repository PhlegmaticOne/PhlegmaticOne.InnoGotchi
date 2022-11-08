using PhlegmaticOne.InnoGotchi.Web.Services.Storage;

namespace PhlegmaticOne.InnoGotchi.Web;

public class CustomAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public CustomAuthenticationMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext httpContext, ILocalStorageService localStorageService)
    {
        var anonymousEndpoints = localStorageService.GetAnonymousEndpoints();

        if (PathIsAnonymous(httpContext.Request.Path, anonymousEndpoints))
        {
            await _next(httpContext);
            return;
        }

        var isAuthenticationRequired = localStorageService.GetIsAuthenticationRequired();
        if (isAuthenticationRequired is true)
        {
            var loginUrl = localStorageService.GetLoginUrl();
            httpContext.Response.Redirect(loginUrl ?? "/");
            return;
        }

        await _next(httpContext);
    }

    private static bool PathIsAnonymous(PathString path, string[]? anonymousEndpoints) => 
        anonymousEndpoints is not null && anonymousEndpoints.Contains(path.Value);
}