using Habr.Security.Helpers.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Habr.Security.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        public byte[] GetHash(string password)
        {
            HashAlgorithm algorithm = SHA256.Create();
            var hashBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hashBytes;
        }

        public string GetHashString(string password)
        {
            var hash = GetHash(password);
            var hashString = String.Join("", hash);
            return hashString;
        }
    }
}
