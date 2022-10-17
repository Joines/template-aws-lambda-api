using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Template.Aws.Lambda.Domain.Results.Bases;
using Template.Aws.Rest.Representation.Responses;
using static Template.Aws.Rest.Representation.Responses.ResultMessage;

namespace Template.Aws.Lambda.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private static readonly JsonSerializerSettings jsonSettings =
            InitJsonSettings();

        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(
            ILogger<ErrorHandlerMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var exceptionFeature = httpContext.Features
                .Get<IExceptionHandlerPathFeature>();

            var restResult = GetResult(exceptionFeature, out var statusCode);
            var errorJson = JsonConvert.SerializeObject(restResult, jsonSettings);

            httpContext.Response.ContentType = "application/json; charset=utf-8";
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(errorJson);
        }

        private Result GetResult(IExceptionHandlerPathFeature exceptionFeature, out int statusCode)
        {
            var restResult = new Result(new List<ResultMessage>());

            switch(exceptionFeature?.Error)
            {
                default:
                    restResult.Messages.Add(
                        GetResultMessageWhenInternalServerError(out statusCode));

                    logger.LogError(1, exceptionFeature?.Error,
                        "{Middleware} - InternalServerError - Message {Message} - Path {Path}",
                        nameof(ErrorHandlerMiddleware), exceptionFeature?.Error.Message ??
                        "no message",
                        exceptionFeature?.Path ?? "no path");
                    break;
            }

            return restResult;
        }

        private ResultMessage GetResultMessageWhenInternalServerError(
            out int statusCode)
        {
            statusCode = StatusCodes.Status500InternalServerError;

            return new ResultMessage(ResultMessageType.Error,
                statusCode.ToString(), "InternalServerError");
        }

        private static JsonSerializerSettings InitJsonSettings()
        {
            var _jsonSettings = new JsonSerializerSettings();

            Startup.ConfigureJsonSettings(_jsonSettings);

            return _jsonSettings;
        }
    }
}
