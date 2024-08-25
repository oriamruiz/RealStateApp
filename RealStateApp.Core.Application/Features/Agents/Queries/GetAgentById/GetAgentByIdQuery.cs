using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Features.Improvements.Queries.GetImprovementById;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Queries.GetAgentById
{
    /// <summary>
    /// Parametro para buscar el agente
    /// </summary>
    public class GetAgentByIdQuery : IRequest<AgentDto>
    {
        /// <example>
        /// a59f1445-8679-4a2d-a8a6-aaaad23b220a
        /// </example>
        [SwaggerParameter(Description ="Id del agente a buscar")]
        public string Id { get; set; }
    }

    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, AgentDto>
    {
        private readonly IAccountService _userService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetAgentByIdQueryHandler(IAccountService userService, IMapper mapper, IPropertyRepository propertyRepository)
        {
            _userService = userService;
            _mapper = mapper;
            _propertyRepository = propertyRepository;
        }

        public async Task<AgentDto> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
        {
            var agents = await _userService.GetAllByRoleAsync(Roles.AGENTE.ToString());

            var agent = agents.FirstOrDefault(a => a.Id == request.Id);

            if (agent == null) return null;

            var agentdto = _mapper.Map<AgentDto>(agent);
            
            var agentproperties = await _propertyRepository.GetAllByAgentIdAsync(agent.Id);

            agentdto.QuantityProperties = agentproperties.Count();
            
            return agentdto;
        }
    }
}
