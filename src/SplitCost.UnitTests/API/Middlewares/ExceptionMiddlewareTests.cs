using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Playground.API.Middlewares;
using SplitCost.API.Middlewares.Strategy;
using System.Text.Json;

namespace SplitCost.UnitTests.API.Middlewares;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenExceptionThrown_UsesHandlerAndWritesResponse()
    {
        // Arrange
        var exceptionMessage = "Test exception";
        var exception = new InvalidOperationException(exceptionMessage);

        var mockHandler = new Mock<IExceptionHandler>();
        mockHandler.Setup(h => h.CanHandle(exception)).Returns(true);
        mockHandler.Setup(h => h.Handle(exception))
                   .Returns(((int)System.Net.HttpStatusCode.InternalServerError, exceptionMessage, (object?)null));

        var mockLogger = Mock.Of<ILogger<ExceptionMiddleware>>();

        var context = new DefaultHttpContext();
        var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        RequestDelegate next = _ => throw exception;

        var middleware = new ExceptionMiddleware(next, mockLogger, new[] { mockHandler.Object });

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        responseBody.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(responseBody);
        var responseJson = await reader.ReadToEndAsync();

        var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseJson);

        // usar camelCase
        Assert.Equal(exceptionMessage, response["message"].ToString());
        Assert.Equal((int)System.Net.HttpStatusCode.InternalServerError, int.Parse(response["statusCode"].ToString()));
        Assert.Null(response["details"]);


        mockHandler.Verify(h => h.CanHandle(exception), Times.Once);
        mockHandler.Verify(h => h.Handle(exception), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenResponseAlreadyStarted_DoesNotWriteResponse()
    {
        // Arrange
        var exception = new Exception("Test exception");

        var mockHandler = new Mock<IExceptionHandler>();
        mockHandler.Setup(h => h.CanHandle(exception)).Returns(true);

        var mockLogger = Mock.Of<ILogger<ExceptionMiddleware>>();

        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;

        // Simular HasStarted = true através de OnStarting
        var hasStarted = true;
        context.Response.OnStarting(() =>
        {
            hasStarted = true;
            return Task.CompletedTask;
        });

        RequestDelegate next = _ => throw exception;

        var middleware = new ExceptionMiddleware(next, mockLogger, new[] { mockHandler.Object });

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        mockHandler.Verify(h => h.CanHandle(It.IsAny<Exception>()), Times.Never);
        mockHandler.Verify(h => h.Handle(It.IsAny<Exception>()), Times.Never);
        Assert.Equal(0, responseStream.Length); // Nenhum dado foi escrito
    }


}

