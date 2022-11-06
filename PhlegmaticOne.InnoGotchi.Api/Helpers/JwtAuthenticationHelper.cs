using System.Text;

namespace PhlegmaticOne.InnoGotchi.Api.Helpers;

internal class JwtAuthenticationHelper
{
    internal static string Issuer = Guid.NewGuid().ToString();
    internal static string Audience = Guid.NewGuid().ToString();

    private static string SecretKey = Guid.NewGuid().ToString();

    internal static byte[] GetSecretBytes() => Encoding.UTF8.GetBytes(SecretKey);
}


