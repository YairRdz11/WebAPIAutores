using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Services
{
    public class HashService
    {
        public ResultHash Hash(string plainText)
        {
            var salt = new byte[16];
            using(var random = RandomNumberGenerator.Create()) { 
                random.GetBytes(salt);
            };

            return Hash(plainText, salt);
        }

        public ResultHash Hash(string plainText, byte[] salt)
        {
            var key = KeyDerivation.Pbkdf2(password: plainText,
                salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(key);

            return new ResultHash()
            {
                Hash = hash,
                Salt = salt
            };
        }
    }
}
