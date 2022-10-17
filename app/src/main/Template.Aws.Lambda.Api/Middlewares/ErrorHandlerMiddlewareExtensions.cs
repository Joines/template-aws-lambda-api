using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.DependencyInjection;
using Template.Aws.Rest.Representation.Responses;
using static Template.Aws.Rest.Representation.Responses.ResultMessage;

namespace Template.Aws.Lambda.Api.Middlewares
{
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomGlobalErrorHandler(
            this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(builder =>
             builder.Run(async (context) =>
            {
                var handlerMiddleware = builder.ApplicationServices
                    .GetService<ErrorHandlerMiddleware>();

                await handlerMiddleware.Invoke(context);
            }));
        }

        public static IServiceCollection ConfigureErrorHandlerServices(
            this IServiceCollection services)
        {
            services.AddTransient<ErrorHandlerMiddleware>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var statusCode = StatusCodes.Status400BadRequest;
                    var fieldMessages = GetFieldMessages(context);
                    var resultMessage = new ResultMessage(ResultMessageType.Error,
                        statusCode.ToString(), "Format error", fieldMessages);

                    return new RestResult(resultMessage, statusCode);
                };
            });

            return services;
        }

        private static List<ResultFieldMessage> GetFieldMessages(ActionContext context)
        {
            var fieldMessages = new List<ResultFieldMessage>();

            foreach(var keyValuePair in context.ModelState)
            {
                if(keyValuePair.Value.ValidationState == ModelValidationState.Invalid)
                {
                    var message = keyValuePair.Value.AttemptedValue == null
                        ? keyValuePair.Value.Errors.SingleOrDefault().ErrorMessage
                        : $"'{keyValuePair.Value.AttemptedValue}' is not valid.";

                    var resultFieldMessage = new ResultFieldMessage(
                        keyValuePair.Key, keyValuePair.Value.AttemptedValue, message);

                    fieldMessages.Add(resultFieldMessage);
                }
            }

            return fieldMessages;
        }
    }
}
