using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Template.Aws.Lambda.Api;

namespace Template.Aws.Lambda.ComponentsTests.Utils
{
    public class WebAppFactory : WebApplicationFactory<Startup>
    {
        public static readonly JsonSerializerSettings JsonSettings =
            InitJsonSettings();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // before program
            // before startup
            builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                // after program
                // after startup
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = TestAuthenticationExtensions.DefaultScheme;
                    options.DefaultForbidScheme = TestAuthenticationExtensions.DefaultScheme;
                    options.DefaultChallengeScheme = TestAuthenticationExtensions.DefaultScheme;
                    options.DefaultAuthenticateScheme = TestAuthenticationExtensions.DefaultScheme;
                })
                .AddTestAuth(_ => { });

                services.AddAuthorization(options =>
                {
                    var testPolicy = new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(TestAuthenticationExtensions.DefaultScheme)
                        .RequireAuthenticatedUser()
                        .Build();

                    options.AddPolicy("TestPolicy", testPolicy);
                    options.DefaultPolicy = testPolicy;
                });
            });

            base.ConfigureWebHost(builder);
        }

        public HttpClient CreateHttpClient(Action<IServiceCollection> services)
        {
            var client = WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services);

                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            })
            .CreateClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "test-jwt");

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            client.DefaultRequestHeaders
                .Add("x-template-correlationId", Guid.NewGuid().ToString());

            return client;
        }

        internal static StringContent GetStringContent(object obj)
            => new StringContent(JsonConvert.SerializeObject(obj, InitJsonSettings()),
                Encoding.UTF8, MediaTypeNames.Application.Json);

        private static JsonSerializerSettings InitJsonSettings()
        {
            var jsonSettings = new JsonSerializerSettings();

            Startup.ConfigureJsonSettings(jsonSettings);

            return jsonSettings;
        }
    }
}
