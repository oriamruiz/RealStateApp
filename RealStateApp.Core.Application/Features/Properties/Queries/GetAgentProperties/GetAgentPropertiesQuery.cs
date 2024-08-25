using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetAgentProperties
{
    /// <summary>
    /// parametro para buscar propiedades de agente
    /// </summary>
    public class GetAgentPropertiesQuery : IRequest<IList<PropertyDto>>
    {
        /// <example>
        /// a59f1445-8679-4a2d-a8a6-aaaad23b220a
        /// </example>
        [SwaggerParameter(Description = "Id del agente a buscar sus propiedades")]
        public string Id { get; set; }
    }

    public class GetAgentPropertiesQueryHandler : IRequestHandler<GetAgentPropertiesQuery, IList<PropertyDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetAgentPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper, IAccountService accountService, IImprovementRepository improvementRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _accountService = accountService;
            _improvementRepository = improvementRepository;
        }
        public async Task<IList<PropertyDto>> Handle(GetAgentPropertiesQuery request, CancellationToken cancellationToken)
        {
            var agents = await _accountService.GetAllByRoleAsync(Roles.AGENTE.ToString());

            var agent = agents.FirstOrDefault(a => a.Id == request.Id);

            if (agent == null) throw new Exception("agent not found");

            var propertylist = await _propertyRepository.GetAllByAgentIdAsync(agent.Id);

            var agentsproperty = _mapper.Map<List<PropertyDto>>(propertylist);

            if (agentsproperty != null && agentsproperty.Count > 0)
            {
                foreach (var property in agentsproperty)
                {
                    
                    property.AgentName = agent.FirstName;

                    var improvements = await _improvementRepository.GetAllAsync();
                    var improvementsid = property.Improvements.Select(pi => pi.ImprovementId).ToList();

                    if (improvements != null && improvements.Count > 0)
                    {
                        foreach (var impid in improvementsid)
                        {
                            var improvement = improvements.FirstOrDefault(i => i.Id == impid);
                            property.PropertyImprovements.Add(improvement);

                        }
                    }
                }
            }

            if (agentsproperty == null || agentsproperty.Count == 0) throw new Exception("properties not found");

            return agentsproperty;
        }


    }
}
