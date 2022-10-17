using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;
using Template.Aws.Lambda.Application.Models;
using Template.Aws.Lambda.Application.Queries;
using Template.Aws.Lambda.ComponentsTests.Utils;
using Template.Aws.Lambda.ComponentsTests.Utils.Models;
using Template.Aws.Lambda.Utils.Tests;
using Template.Aws.Lambda.Utils.Tests.Domain;

namespace Template.Aws.Lambda.ComponentsTests.Web.Controllers
{
    public class ClientController: IDisposable, IClassFixture<WebAppFactory>
    {
        private const string baseUrl = "/clients";
        private const string allUrl = baseUrl + "/all";

        private readonly Mock<IMediator> mediator;

        private readonly HttpClient client;

        public ClientController(WebAppFactory webAppFactory)
        {
            mediator = new Mock<IMediator>();

            client = webAppFactory.CreateHttpClient(services =>
            {
                services.AddScoped(_ => mediator.Object);
            });
        }

        [Fact]
        public async Task GetAllClients_Sucess()
        {
            // Arrange --------------------------------------------------------
            var expecteClientResponse = ClientBuilder.Build();

            var uri = new Uri(allUrl, UriKind.Relative);

            _ = mediator.Setup(mock => mock.Send(
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
                .Callback<IRequest<GetClientResponse>, CancellationToken>(
                (requestCb, cancellationToken) =>
                {
                    var query = (GetAllClientQuery)requestCb;

                    query.Should()
                        .NotBeNull();
                })
                .ReturnsAsync(expecteClientResponse);

            // Act -------------------------------------------------------------
            
            // Utilizar quando for post
            //using var emptyBody = StringContentBuilder
            //    .Build(WebAppFactory.JsonSettings);

            using var httpResponse = await client.GetAsync(uri);

            var json = await httpResponse.Content.ReadAsStringAsync();

            var response = JsonConvert
                .DeserializeObject<ResultData<GetClientResponse>>(
                    json, WebAppFactory.JsonSettings);

            // Assert ------------------------------------------------------------
            httpResponse.StatusCode.Should()
                .Be(HttpStatusCode.OK);

            response.Should()
                .NotBeNull();

            mediator.Verify(
                mock => mock.Send(It.IsAny<IRequest<GetClientResponse>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllClients_ThrowException()
        {
            // Arrange --------------------------------------------------------         
            var uri = new Uri(allUrl, UriKind.Relative);

            _ = mediator.Setup(mock => mock.Send(
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
                .Callback<IRequest<GetClientResponse>, CancellationToken>(
                (requestCb, cancellationToken) =>
                {
                    var query = (GetAllClientQuery)requestCb;

                    query.Should()
                        .NotBeNull();
                })
                .ThrowsAsync(new InvalidOperationException("test ex"));

            // Act -------------------------------------------------------------
            using var httpResponse = await client.GetAsync(uri);

            var json = await httpResponse.Content.ReadAsStringAsync();

            var response = JsonConvert
                .DeserializeObject<ResultData<GetClientResponse>>(
                    json, WebAppFactory.JsonSettings);

            // Assert ------------------------------------------------------------
            httpResponse.StatusCode.Should()
                .Be(HttpStatusCode.InternalServerError);

            mediator.Verify(
                mock => mock.Send(It.IsAny<IRequest<GetClientResponse>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        public void Dispose() =>
            client?.Dispose();
    }
}
