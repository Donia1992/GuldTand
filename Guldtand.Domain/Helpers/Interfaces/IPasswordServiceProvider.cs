namespace Guldtand.Domain.Helpers
{
    public interface IPasswordServiceProvider
    {
        PasswordServiceProvider.EncryptedPasswordObject CreatePasswordHash(string password);
        bool VerifyPasswordHash(string password, PasswordServiceProvider.EncryptedPasswordObject encryptedPassword);
    }
}