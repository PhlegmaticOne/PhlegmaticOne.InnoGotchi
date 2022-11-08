namespace PhlegmaticOne.InnoGotchi.Web.Extentions;

public static class UriExtensions
{
    public static Uri Combine(this Uri uri, string nextUri) => new(uri, nextUri);
}