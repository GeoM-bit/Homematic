using HomematicApp.Domain.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
        int bytesRequested;
        int iterations;

        public HashService(IConfiguration configuration)
        {
            _configuration = configuration;
            hashAlgorithm = HashAlgorithmName.SHA512;
            bytesRequested = int.Parse(_configuration["HashOptions:BytesRequested"]);
            iterations = int.Parse(_configuration["HashOptions:Iterations"]);
            salt = _configuration["HashOptions:Salt"].Split("-")
                   .Select(t => byte.Parse(t, NumberStyles.AllowHexSpecifier))
                   .ToArray();
        }
       

        public string HashPassword(string password)
        {
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: password!,
                                                salt: salt,
                                                prf: KeyDerivationPrf.HMACSHA512,
                                                iterationCount: iterations,
                                                numBytesRequested: bytesRequested));
            return hash;       
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var hashToCompare = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: password!,
                                                salt: salt,
                                                prf: KeyDerivationPrf.HMACSHA512,
                                                iterationCount: iterations,
                                                numBytesRequested: bytesRequested));

            return hashToCompare.Equals(passwordHash);
        }
    }
}
