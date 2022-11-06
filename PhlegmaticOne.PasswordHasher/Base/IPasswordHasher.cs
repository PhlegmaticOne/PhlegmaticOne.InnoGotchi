namespace PhlegmaticOne.PasswordHasher.Base;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}