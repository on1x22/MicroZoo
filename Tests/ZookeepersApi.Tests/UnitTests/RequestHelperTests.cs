using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Specialities;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class RequestHelperTests
    {
        private Mock<HttpMessageHandler> GetMockHandler(out Animal animal, HttpStatusCode statusCode)
        {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            animal = new Fixture().Build<Animal>().Without(p => p.AnimalType).Create();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = JsonContent.Create(animal)
            };

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            return mockHandler;
        }

        [Fact]
        public async void GetResponseAsync_should_return_content()
        {
            // Arrange    
            string connectionString = "http://test";
            var mockHandler = GetMockHandler(out var animal, HttpStatusCode.OK);

            var httpClient = new HttpClient(mockHandler.Object);
            var requestHelper = new RequestHelper(httpClient);

            // Act
            var actual = await requestHelper.GetResponseAsync<Animal>(method: HttpMethod.Get,
                                                    requestUri: connectionString);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(animal.Name, actual.Name);
        }

        [Fact]
        public async void GetResponseAsync_pass_invalid_Uri_scheme_should_return_null()
        {
            // Arrange    
            string connectionString = "ht";

            var httpClient = new HttpClient();
            var requestHelper = new RequestHelper(httpClient);

            // Act
            var actual = await requestHelper.GetResponseAsync<Animal>(method: HttpMethod.Get,
                                                    requestUri: connectionString);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public async void GetResponseAsync_get_status_code_NotFound_should_return_null()
        {
            // Arrange    
            string connectionString = "http://test";            
            var mockHandler = GetMockHandler(out var _, HttpStatusCode.NotFound);

            var httpClient = new HttpClient(mockHandler.Object);
            var requestHelper = new RequestHelper(httpClient);

            // Act
            var actual = await requestHelper.GetResponseAsync<Animal>(method: HttpMethod.Get,
                                                    requestUri: connectionString);

            //Assert
            Assert.Null(actual);
        }
    }
}
