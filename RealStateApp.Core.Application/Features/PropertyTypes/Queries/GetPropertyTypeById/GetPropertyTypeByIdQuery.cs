using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeByIdQuery
{
    /// <summary>
    /// parametros para filtrar tipo de producto
    /// </summary>
    public class GetPropertyTypeByIdQuery : IRequest<PropertyTypeDto>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "id del tipo de producto a buscar")]
        public int Id { get; set; }
    }

    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<PropertyTypeDto> Handle(GetPropertyTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var propertytype = await _propertyTypeRepository.GetByIdAsync(request.Id);

            if (propertytype == null) throw new Exception("property type not found");

            var propertytypedtp = _mapper.Map<PropertyTypeDto>(propertytype);

            return propertytypedtp;
        }
    }

}
