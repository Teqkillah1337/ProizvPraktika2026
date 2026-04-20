using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using RequestsApp;
using SupabaseAuthLib;

namespace RequestsApp.Tests
{
    [TestClass]
    public class SupabaseAuthTests
    {
        [TestMethod]
        public async Task SignUp_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            string invalidEmail = "not-an-email";
            string password = "123456";

            // Act
            var result = await SupabaseAuth.SignUp(invalidEmail, password);
            bool success = result.success;
            string message = result.message;

            // Assert
            Assert.IsFalse(success);
            StringAssert.Contains(message, "Ошибка");
        }

        [TestMethod]
        public async Task SignIn_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string email = "nonexistent@test.com";
            string password = "wrongpassword";

            // Act
            var result = await SupabaseAuth.SignIn(email, password);
            bool success = result.success;
            string message = result.message;

            // Assert
            Assert.IsFalse(success);
            Assert.AreEqual("Неверный email или пароль", message);
        }

        [TestMethod]
        public async Task SignUp_EmptyEmail_ReturnsFalse()
        {
            // Arrange
            string email = "";
            string password = "123456";

            // Act
            var result = await SupabaseAuth.SignUp(email, password);
            bool success = result.success;

            // Assert
            Assert.IsFalse(success);
        }

        [TestMethod]
        public async Task SignUp_EmptyPassword_ReturnsFalse()
        {
            // Arrange
            string email = "test@test.com";
            string password = "";

            // Act
            var result = await SupabaseAuth.SignUp(email, password);
            bool success = result.success;

            // Assert
            Assert.IsFalse(success);
        }
    }
}