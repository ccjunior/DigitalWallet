using DigitalWallet.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Tests.Domain
{
    public class PasswordHasherTests
    {
        [Fact]
        public void HashPassword_ValidPassword_ReturnsValidHash()
        {
            string password = "TestPassword123!";

            string hash = PasswordHasher.HashPassword(password);

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.Matches("^[0-9a-f]+$", hash); 
        }

        [Fact]
        public void HashPassword_SamePassword_ReturnsSameHash()
        {
            string password = "TestPassword123!";

            string hash1 = PasswordHasher.HashPassword(password);
            string hash2 = PasswordHasher.HashPassword(password);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void HashPassword_DifferentPasswords_ReturnDifferentHashes()
        {
            string password1 = "TestPassword123!";
            string password2 = "TestPassword123";

            string hash1 = PasswordHasher.HashPassword(password1);
            string hash2 = PasswordHasher.HashPassword(password2);

            Assert.NotEqual(hash1, hash2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void HashPassword_NullOrEmptyPassword_ThrowsArgumentException(string password)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                PasswordHasher.HashPassword(password));

            Assert.Equal("Password cannot be null or empty.", exception.Message);
        }
    }
}
