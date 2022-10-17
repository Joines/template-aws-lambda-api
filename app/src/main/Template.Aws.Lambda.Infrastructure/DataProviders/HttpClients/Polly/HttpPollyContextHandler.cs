using Template.Aws.Lambda.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Template.Aws.Lambda.Infrastructure.DataProviders.HttpClients.Polly
{
    public class HttpPollyContextHandler: DelegatingHandler
    {
        private readonly ILogger<HttpPollyContextHandler> logger;

        public HttpPollyContextHandler(
            ILogger<HttpPollyContextHandler> logger)
        {
            this.logger = logger;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var context = request.GetPolicyExecutionContext();
            
            if(context is null)
            {
                context = new Context();
                request.SetPolicyExecutionContext(context);
            }
            
            context[PollyContextExtensions.PollyContextLoggerKey] = logger;
            
            return base.SendAsync(request, cancellationToken);
        }
    }
}
