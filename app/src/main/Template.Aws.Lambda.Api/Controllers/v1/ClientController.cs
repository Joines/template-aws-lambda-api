using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Template.AuthenticatedUserInfo.Controllers;
using System.Net.Mime;
using Template.Aws.Lambda.Application.Queries;

namespace Template.Aws.Lambda.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("/template/clients")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ClientController: AuthenticatedController 
    {
        private readonly ILogger<ClientController> logger;
        private readonly IMediator mediator;
        
        public ClientController(ILogger<ClientController> logger,
            IMediator mediator) 
            : base(logger)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpGet("all", Name = "all-clients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync(
            [FromServices] GetAllClientQuery query,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("{class} - {method} - Query '{query}'",
                nameof(ClientController),
                nameof(ClientController.GetAllAsync), query);

            var response = await mediator.Send(query, cancellationToken);

            return Ok(response);
        }
    }
}
