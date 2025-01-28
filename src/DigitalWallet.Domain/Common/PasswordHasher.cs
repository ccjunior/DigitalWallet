using System.Security.Cryptography;
using System.Text;

namespace DigitalWallet.Domain.Common
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.");

            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                var hashString = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    hashString.Append(b.ToString("x2")); 
                }

                return hashString.ToString();
            }
        }
    }
}
