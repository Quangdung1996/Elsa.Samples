using Elsa.Samples.Interfaces;
using Elsa.Samples.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Elsa.Samples.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public HashedPassword HashPassword(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return HashPassword(password, salt);
        }

        public HashedPassword HashPassword(string password, byte[] salt)
        {
            var hashed = KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return new HashedPassword(hashed, salt);
        }
    }
}