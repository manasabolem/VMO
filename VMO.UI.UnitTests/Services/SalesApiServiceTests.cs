using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VMO.UI.Models;
using VMO.UI.Services;

namespace VMO.UI.UnitTests.Services
{
    [TestFixture]
    public class SalesApiServiceTests
    {
        private Mock<ILogger<SalesApiService>> _mockLogger;
        private SalesApiService _salesApiService;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<SalesApiService>>();

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.example.com/")
            };

            _salesApiService = new SalesApiService(_httpClient, _mockLogger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
            _mockHttpMessageHandler = null;
            _salesApiService = null;
            _mockLogger = null;
        }

        [Test]
        public async Task GetSalesDataAsync_ReturnsSalesData_WhenValidResponse()
        {
            var mockSalesData = new List<SalesDataViewModel>
            {
                new SalesDataViewModel { Product = "Product1", UnitsSold = "100" },
                new SalesDataViewModel { Product = "Product2", UnitsSold = "150" }
            };

            var jsonResponse = JsonConvert.SerializeObject(mockSalesData);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var result = await _salesApiService.GetSalesDataAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Product1", result[0].Product);
        }

        [Test]
        public async Task GetSalesDataAsync_ThrowsException_WhenResponseIsEmpty()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var ex = Assert.ThrowsAsync<Exception>(async () => await _salesApiService.GetSalesDataAsync());
            Assert.AreEqual("Cannot get Sales Data", ex.Message);
        }

        [Test]
        public async Task GetSalesDataAsync_ThrowsException_WhenresponseIsNull()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = null
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var ex = Assert.ThrowsAsync<Exception>(async () => await _salesApiService.GetSalesDataAsync());
            Assert.AreEqual("Cannot get Sales Data", ex.Message);
        }

        [Test]
        public async Task GetSalesDataAsync_LogsError_WhenExceptionIsThrown()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var ex = Assert.ThrowsAsync<HttpRequestException>(async () => await _salesApiService.GetSalesDataAsync());
            Assert.AreEqual("Response status code does not indicate success: 500 (Internal Server Error).", ex.Message);

        }
    }
}
