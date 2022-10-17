using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Template.Aws.Lambda.Application.Models;
using Template.Aws.Lambda.Application.Queries;
using Template.Aws.Lambda.Application.Repositories;
using Template.Aws.Lambda.Domain.Entities;
using Template.Aws.Lambda.Domain.ValuesObjects;

namespace Template.Aws.Lambda.Application.Handlers.QueriesHandlers
{
    public class ClientQueryHandle 
        : IRequestHandler<GetAllClientQuery, GetClientResponse> 
    {
        private readonly IMapper mapper;
        private readonly ILogger<ClientQueryHandle> logger;
        private readonly IClientRepository repository;

        public ClientQueryHandle(
            ILogger<ClientQueryHandle> logger,
            IMapper mapper,
            IClientRepository repository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<GetClientResponse> Handle(GetAllClientQuery request,
            CancellationToken cancellationToken)
        {
            var clients = await repository.GetAllClients();

            var response = mapper.Map<GetClientResponse>(clients);

            logger.LogInformation("{class} - {method} - Trace - " +
                "Response {@response}",
                nameof(ClientQueryHandle),
                nameof(Handle), response);

            return response;
        }
    }
}
