using System;
using System.Security.Cryptography;
using System.Text;

namespace Guldtand.Domain.Helpers
{
    public class PasswordServiceProvider : IPasswordServiceProvider
    {
        public EncryptedPasswordObject CreatePasswordHash(string password)
        {
            if (password == null)
                throw new ArgumentNullException($"Parameter {nameof(password)} cannot be null");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"Parameter {nameof(password)} cannot be empty or only contain whitespace.");

            EncryptedPasswordObject encryptedPassword = new EncryptedPasswordObject();

            using (var hmac = new HMACSHA512())
            {
                encryptedPassword.Salt = hmac.Key;
                encryptedPassword.Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            return encryptedPassword;
        }

        public bool VerifyPasswordHash(string password, EncryptedPasswordObject encryptedPassword)
        {
            if (password == null)
                throw new ArgumentNullException($"Parameter {nameof(password)} cannot be null");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"Parameter {nameof(password)} cannot be empty or only contain whitespace.");

            if (encryptedPassword.Hash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (encryptedPassword.Salt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new HMACSHA512(encryptedPassword.Salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != encryptedPassword.Hash[i]) return false;
                }
            }

            return true;
        }

        public class EncryptedPasswordObject
        {
            public byte[] Salt { get; set; }
            public byte[] Hash { get; set; }
        }
    }
}
