
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Mime;
using Template.Aws.Lambda.Api.Middlewares;

namespace Template.Aws.Lambda.ComponentsTests.Web.Middlewares
{
    public class ErrorHandlerMiddlewareTests
    {
        private readonly ILogger<ErrorHandlerMiddleware> mockLogger;

        private readonly ErrorHandlerMiddleware handler;

        public ErrorHandlerMiddlewareTests()
        {
            mockLogger = Mock.Of<ILogger<ErrorHandlerMiddleware>>();

            handler = new ErrorHandlerMiddleware(mockLogger);
        }

        [Fact(DisplayName = nameof(ErrorHandlerMiddleware) +
            " contentType: Should be application/json")]
        public async Task ErrorHandlerMiddleware_ContentType_ShouldBeApplicationJson()
        {
            // Arrange --------------------------------------------------------

            var httpContext = HttpContextBuilder.Build<Exception>();

            // Act ------------------------------------------------------------

            await handler.Invoke(httpContext);

            httpContext.Response.ContentType.Should()
                .StartWith(MediaTypeNames.Application.Json);
        }

    }
}
