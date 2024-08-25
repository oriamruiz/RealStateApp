using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Features.Properties.Queries.GetProperty;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetProperty
{
    /// <summary>
    /// parametros para filtrar propiedades
    /// </summary>
    public class GetPropertyQuery : IRequest<PropertyDto>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "id de la propiedad a buscar")]
        public int? Id { get; set; }
        /// <example>
        /// 919164
        /// </example>
        [SwaggerParameter(Description = "id de la propiedad a buscar")]
        public string? Code { get; set; }
    }

    public class GetPropertyQueryHandler : IRequestHandler<GetPropertyQuery, PropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetPropertyQueryHandler(IPropertyRepository propertyRepository, IMapper mapper, IAccountService accountService, IImprovementRepository improvementRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _accountService = accountService;
            _improvementRepository = improvementRepository;
        }
        public async Task<PropertyDto> Handle(GetPropertyQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<GetPropertyParameter>(request);

            var propertydto = await GetPropertyByFilters(filter);

            if (propertydto == null) throw new Exception("property not found");

            return propertydto;
        }



        private async Task<PropertyDto> GetPropertyByFilters (GetPropertyParameter filter)
        {
            var properties = await _propertyRepository.GetAllAsync();

            Property property = null;

            if(filter.Id > 0)
            {
                property = properties.FirstOrDefault(p => p.Id == filter.Id);

            }
            
            if(!string.IsNullOrEmpty(filter.Code))
            {
                property = properties.FirstOrDefault(p => p.Code == filter.Code);

            }

            var propertydto = _mapper.Map<PropertyDto>(property);

            var agents = await _accountService.GetAllByRoleAsync(Roles.AGENTE.ToString());

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

            return propertydto;

        }
    }
}
