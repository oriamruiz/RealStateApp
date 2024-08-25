using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<IList<PropertyDto>>
    {

    }

    public class GetAllPropertiesHandler : IRequestHandler<GetAllPropertiesQuery, IList<PropertyDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetAllPropertiesHandler(IPropertyRepository propertyRepository, IMapper mapper, IAccountService accountService, IImprovementRepository improvementRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _accountService = accountService;
            _improvementRepository = improvementRepository;
        }
        public async Task<IList<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var propertylist = await _propertyRepository.GetAllAsync();

            var propertiesdto = _mapper.Map<List<PropertyDto>>(propertylist);

            if (propertiesdto != null && propertiesdto.Count > 0)
            {
                var agents = await _accountService.GetAllByRoleAsync(Roles.AGENTE.ToString());
                 
                foreach (var propertydto in propertiesdto)
                {
                    var agent = agents.FirstOrDefault(a => a.Id == propertydto.AgentId);

                    if (agent != null)
                    {
                        propertydto.AgentName = agent.FirstName;
                    }

                    var improvements = await _improvementRepository.GetAllAsync();
                    var improvementsid = propertydto.Improvements.Select(pi => pi.ImprovementId).ToList();

                    if (improvements != null && improvements.Count > 0)
                    {
                        foreach (var impid in improvementsid)
                        {
                            var improvement = improvements.FirstOrDefault(i => i.Id == impid);
                            propertydto.PropertyImprovements.Add(improvement);

                        }
                    }
                }
            }

            if (propertiesdto == null || propertiesdto.Count == 0) throw new Exception("properties not found");

            return propertiesdto;
        }

        
    }

}
