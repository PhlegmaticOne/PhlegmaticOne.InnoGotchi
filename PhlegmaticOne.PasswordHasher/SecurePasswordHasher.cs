using PhlegmaticOne.PasswordHasher.Base;
using System.Security.Cryptography;
using System.Text;

namespace PhlegmaticOne.PasswordHasher;

public class SecurePasswordHasher : IPasswordHasher
{
    public string Hash(string password) => HashPrivate(password);

    public bool Verify(string password, string hashedPassword) => 
        HashPrivate(password) == hashedPassword;

    private static string HashPrivate(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.Default.GetBytes(password);
        var hashed = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hashed);
    }
}