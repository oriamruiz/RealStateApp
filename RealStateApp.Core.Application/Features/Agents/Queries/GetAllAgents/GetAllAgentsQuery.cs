using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Agents.Queries.GetAllAgents
{
    
    public class GetAllAgentsQuery : IRequest<IList<AgentDto>>
    {

    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, IList<AgentDto>>
    {
        private readonly IAccountService _userService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetAllAgentsQueryHandler(IAccountService userService, IMapper mapper, IPropertyRepository propertyRepository)
        {
            _userService = userService;
            _mapper = mapper;
            _propertyRepository = propertyRepository;
        }

        public async Task<IList<AgentDto>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            var agents = await _userService.GetAllByRoleAsync(Roles.AGENTE.ToString());

            if (agents == null || agents.Count == 0) throw new Exception("Agents not found");

            var agentsdto = _mapper.Map<List<AgentDto>>(agents);

           

            foreach (var agentdto in agentsdto)
            {
                var agentproperties = await _propertyRepository.GetAllByAgentIdAsync(agentdto.Id);

                agentdto.QuantityProperties = agentproperties.Count();
            }

            return agentsdto;
        }


    }
}
