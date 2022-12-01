namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;

public static class UriExtensions
{
    public static Uri Combine(this Uri uri, string nextUri) => new(uri, nextUri);
}