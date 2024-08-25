using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Features.Properties.Queries.GetProperty;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;
using System.Collections.Generic;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypesQuery
{
    public class GetAllPropertyTypesQuery : IRequest<IList<PropertyTypeDto>>
    {
       
    }

    public class GetAllPropertyTypesQueryHandler : IRequestHandler<GetAllPropertyTypesQuery, IList<PropertyTypeDto>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetAllPropertyTypesQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<IList<PropertyTypeDto>> Handle(GetAllPropertyTypesQuery request, CancellationToken cancellationToken)
        {
            var propeertytypes = await _propertyTypeRepository.GetAllAsync();

            if (propeertytypes == null || propeertytypes.Count == 0) throw new Exception("property types not found");

            var properttypesdto = _mapper.Map<List<PropertyTypeDto>>(propeertytypes);

            return properttypesdto;
        }

       
    }
}
