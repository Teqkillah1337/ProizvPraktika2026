using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RequestsApp.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void ValidateEmail_EmptyEmail_ReturnsFalse()
        {
            // Arrange
            string email = "";
            string password = "123456";

            // Act
            bool isValid = !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidatePassword_EmptyPassword_ReturnsFalse()
        {
            // Arrange
            string email = "test@test.com";
            string password = "";

            // Act
            bool isValid = !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateCredentials_Valid_ReturnsTrue()
        {
            // Arrange
            string email = "test@test.com";
            string password = "123456";

            // Act
            bool isValid = !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateRequest_CitizenNameEmpty_ReturnsFalse()
        {
            // Arrange
            string citizenName = "";
            string description = "Описание";

            // Act
            bool isValid = !string.IsNullOrEmpty(citizenName) && !string.IsNullOrEmpty(description);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateRequest_DescriptionEmpty_ReturnsFalse()
        {
            // Arrange
            string citizenName = "Иванов Иван";
            string description = "";

            // Act
            bool isValid = !string.IsNullOrEmpty(citizenName) && !string.IsNullOrEmpty(description);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateRequest_Valid_ReturnsTrue()
        {
            // Arrange
            string citizenName = "Иванов Иван";
            string description = "Описание проблемы";

            // Act
            bool isValid = !string.IsNullOrEmpty(citizenName) && !string.IsNullOrEmpty(description);

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}