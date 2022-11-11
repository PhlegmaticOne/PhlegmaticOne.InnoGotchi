namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;

public static class ListExtensions
{
    public static List<T> With<T>(this List<T> source, IEnumerable<T> data)
    {
        source.AddRange(data);
        return source;
    }
}