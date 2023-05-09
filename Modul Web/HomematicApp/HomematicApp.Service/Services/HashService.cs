using HomematicApp.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Security.Cryptography;

namespace HomematicApp.Service.Services
{
    public class HashService : IHashService
    {
        HashAlgorithmName hashAlgorithm;
        private readonly IConfiguration _configuration;
        byte[] salt;
        int keySize;
        int iterations;

        public HashService(IConfiguration configuration)
        {
            _configuration = configuration;
            hashAlgorithm = HashAlgorithmName.SHA512;
            keySize = int.Parse(_configuration["HashOptions:KeySize"]);
            iterations = int.Parse(_configuration["HashOptions:Iterations"]);
            salt = _configuration["HashOptions:Salt"].Split("-")
                   .Select(t => byte.Parse(t, NumberStyles.AllowHexSpecifier))
                   .ToArray();
        }
       

        public string HashPassword(string password)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    hashAlgorithm,
                    keySize);

            return Convert.ToHexString(hash);       
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(passwordHash));
        }
    }
}
