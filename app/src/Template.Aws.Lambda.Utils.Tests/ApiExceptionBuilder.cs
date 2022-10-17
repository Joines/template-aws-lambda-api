using Refit;
using System.Net;

namespace Template.Aws.Lambda.Utils.Tests
{
    public class ApiExceptionBuilder
    {
        private ApiExceptionBuilder() { }

        public static ApiException Build(HttpStatusCode statusCode) =>
            BuildAsync(statusCode)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

        public static Task<ApiException> BuildAsync(HttpStatusCode statusCode) =>
            ApiException.Create(new HttpRequestMessage(),
                HttpMethod.Get, new HttpResponseMessage(statusCode));
    }
}