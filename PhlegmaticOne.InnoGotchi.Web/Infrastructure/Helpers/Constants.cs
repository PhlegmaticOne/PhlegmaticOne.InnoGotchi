using System.Text.RegularExpressions;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Helpers;

internal static class Constants
{
    internal const string HomeUrl = "/";
    internal const string CookieAuthenticationType = "ApplicationCookie";
    internal static Regex PasswordRegex => 
        new("(?=^.{8,}$)(?=.*\\d)(?=.*[!@#$%^&*]+)(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$");
}