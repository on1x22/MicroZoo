

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Models;
using Moq.Protected;
using System.Net.Http.Json;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class RequestHelperTests
    {
        [Fact]
        public async void GetResponseAsync_should_return_content()
        {
            // Arrange    
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var animal = new Fixture().Build<Animal>().Without(p => p.AnimalType).Create();
            var mockResponse = new HttpResponseMessage 
            { 
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonContent.Create<Animal>(animal) 
            };

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var httpClient = new HttpClient(mockHandler.Object);
            var sut = new RequestHelper(httpClient);

            // Act
            var actual = await sut.GetResponseAsync<Animal>(method: HttpMethod.Get,
                                                    requestUri: "http://test");

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(animal.Name, actual.Name);
        }
    }
}
