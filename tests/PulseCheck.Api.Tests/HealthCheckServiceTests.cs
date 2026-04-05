using System.Net;
using PulseCheck.Api.Models;
using PulseCheck.Api.Services;

namespace PulseCheck.Api.Tests;

public class HealthCheckServiceTests
{

    private class FakeHttpHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;

        public FakeHttpHandler(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode);
            return Task.FromResult(response);
        }
    }

    private class FakeExceptionHandler  : HttpMessageHandler
    {
       
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
           throw new HttpRequestException("Server is dead");
        }
    }

    [Fact]
public async Task CheckAsync_WhenUrlReturns200_ShouldBeUp()
{
    // Arrange — set up the ingredients
    var handler = new FakeHttpHandler(HttpStatusCode.OK);
    var httpClient = new HttpClient(handler);
    var service = new HealthCheckService(httpClient);
    var endpoint = new MonitoredEndpoint
    {
        Id = 1,
        Name = "Test",
        Url = "https://fake-url.com"
    };

    // Act — do the thing
    var result = await service.CheckAsync(endpoint);

    // Assert — check the result
    Assert.True(result.IsUp);
    Assert.Equal(200, result.StatusCode);
}

    [Fact]
public async Task CheckAsync_WhenUrlThrowsException_ShouldBeDown()
{
    // Arrange
    var handler = new FakeExceptionHandler();
    var httpClient = new HttpClient(handler);
    var service = new HealthCheckService(httpClient);
    var endpoint = new MonitoredEndpoint
    {
        Id = 1,
        Name = "Test",
        Url = "https://fake-url.com"
    };

    // Act
    var result = await service.CheckAsync(endpoint);

    // Assert
    Assert.False(result.IsUp);
    Assert.Equal(0, result.StatusCode);
    Assert.Equal(0, result.ResponseTimeMs);
}

[Fact]
public async Task CheckAsync_WhenUrlReturns404_ShouldBeDown()
{
    // Arrange — set up the ingredients
    var handler = new FakeHttpHandler(HttpStatusCode.NotFound);
    var httpClient = new HttpClient(handler);
    var service = new HealthCheckService(httpClient);
    var endpoint = new MonitoredEndpoint
    {
        Id = 1,
        Name = "Test",
        Url = "https://fake-url.com"
    };

    // Act — do the thing
    var result = await service.CheckAsync(endpoint);

    // Assert — check the result
    Assert.False(result.IsUp);
    Assert.Equal(404, result.StatusCode);
}



}
