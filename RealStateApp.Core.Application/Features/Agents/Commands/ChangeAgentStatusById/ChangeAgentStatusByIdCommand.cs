using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Commands.ChangeAgentStatusById
{
    /// <summary>
    /// parametros para cambiar estado de agente
    /// </summary>
    public class ChangeAgentStatusByIdCommand : IRequest<string>
    {
        /// <example>
        /// a59f1445-8679-4a2d-a8a6-aaaad23b220a
        /// </example>
        [SwaggerParameter(Description = "Id del agente a cambiar estado")]
        public string Id { get; set; }

        /// <example>
        /// True = activo, false = inactivo
        /// </example>
        [SwaggerParameter(Description = "Estado a cambiar")]
        public bool Status { get; set; }
    }

    public class ChangeAgentStatusByIdCommandHandler : IRequestHandler<ChangeAgentStatusByIdCommand, string>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ChangeAgentStatusByIdCommandHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<string> Handle(ChangeAgentStatusByIdCommand command, CancellationToken cancellationToken)
        {
            var agents = await _accountService.GetAllByRoleAsync(Roles.AGENTE.ToString());

            var agent = agents.FirstOrDefault(a => a.Id == command.Id);

            if (agent == null) throw new Exception("agent not found");

            agent.IsActive = command.Status;
            agent.EmailConfirmed = command.Status;

            ChangeStatusRequest statusRequest = _mapper.Map<ChangeStatusRequest>(agent);

            await _accountService.ChangeStatusAsync(statusRequest);
            
            return agent.Id;
        }
    }
}
