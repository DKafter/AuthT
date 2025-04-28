namespace AuthExample.Core.Abstraction.Interfaces
{
    public interface IPasswordHasher
    {
        (string PasswordHash, string Salt) HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash, string salt);
    }
}
