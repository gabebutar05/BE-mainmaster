using API_Dinamis.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace API_Dinamis.Repository
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 128 / 8;
        private const int KeySize = 256 / 8;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';
        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string passwordHash, string inputPassword)
        {
            var elements = passwordHash.Split(Delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var hashinput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashinput);
        }
    }
}
