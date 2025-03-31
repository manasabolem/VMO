using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VMO.UI.Controllers;
using VMO.UI.Models;
using VMO.UI.Services.Interfaces;

namespace VMO.UI.UnitTests.Controllers
{
    [TestFixture]
    public class SalesControllerTests
    {
        private Mock<ISalesApiService> _mockSalesApiService;
        private Mock<ILogger<SalesController>> _mockLogger;
        private SalesController _salesController;

        [SetUp]
        public void Setup()
        {
            _mockSalesApiService = new Mock<ISalesApiService>();
            _mockLogger = new Mock<ILogger<SalesController>>();

            _salesController = new SalesController(_mockLogger.Object, _mockSalesApiService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _salesController.Dispose(); 
            _mockSalesApiService = null;
            _mockLogger = null;
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithSalesData()
        {
            // Arrange
            var mockSalesData = new List<SalesDataViewModel>
            {
                 new SalesDataViewModel { Segment = "Gov", Country = "Canada", Product = "Mouse", DiscountBand = "none", UnitsSold = "16.15", ManufacturingPrice = "3.00", SalePrice = "2.00", Date =DateTime.Now },
                 new SalesDataViewModel { Segment = "Private", Country = "UK", Product = "keyborad", DiscountBand = "none", UnitsSold = "17.15", ManufacturingPrice = "4.00", SalePrice = "3.00", Date =DateTime.Now   }
            };

            _mockSalesApiService.Setup(service => service.GetSalesDataAsync()).ReturnsAsync(mockSalesData);  

            // Act
            var result = await _salesController.Index();

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(mockSalesData, viewResult.Model);
        }

        [Test]
        public void Index_ThrowsException_LogsError()
        {
            // Arrange
            _mockSalesApiService.Setup(service => service.GetSalesDataAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _salesController.Index());
            Assert.AreEqual("Test exception", ex.Message);
        }
    }
}
