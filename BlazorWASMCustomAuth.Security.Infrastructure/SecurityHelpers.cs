using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BlazorWASMCustomAuth.Security.Infrastructure
{
    public static class SecurityHelpers
    {   
        public static byte[] RandomizeSalt => RandomNumberGenerator.GetBytes(64);

        public static string HashPasword(string password, byte[] salt)
        {
            //var encodedsalt = Encoding.UTF8.GetBytes(salt);

            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            //byte[] salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }
    }
}