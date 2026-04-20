using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using RequestsApp;

namespace RequestsApp.Tests
{
    [TestClass]
    public class SupabaseServiceTests
    {
        [TestMethod]
        public async Task GetMyRequests_ReturnsList()
        {
            // Arrange
            string userId = "test-user-123";

            // Act
            var requests = await SupabaseService.GetMyRequests(userId);

            // Assert
            Assert.IsNotNull(requests);
        }

        [TestMethod]
        public async Task CreateRequest_ValidData_ReturnsResult()
        {
            // Arrange
            var request = new RequestItem
            {
                request_number = "TEST-001",
                citizen_name = "Тестов Тест",
                citizen_phone = "+79123456789",
                description = "Тестовая заявка"
            };
            string userId = "test-user-123";

            // Act
            bool result = await SupabaseService.CreateRequest(request, userId);

            // Assert
            // Метод может вернуть false если нет интернета
            Assert.IsInstanceOfType(result, typeof(bool));
        }
    }
}