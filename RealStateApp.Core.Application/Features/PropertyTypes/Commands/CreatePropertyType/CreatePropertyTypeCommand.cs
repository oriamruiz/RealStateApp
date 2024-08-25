using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType
{
    /// <summary>
    /// Parametros para la creacion de un tipo de propiedad
    /// </summary>
    public class CreatePropertyTypeCommand : IRequest<int>
    {
        /// <example>
        /// Apartamento
        /// </example>
        [SwaggerParameter(Description = "nombre del tipo de propiedad")]
        public string Name { get; set; }
        /// <example>
        /// Una unidad de vivienda independiente ubicada dentro de un edificio de múltiples pisos.....
        /// </example>
        [SwaggerParameter(Description = "descripcion del tipo de propiedad")]
        public string Description { get; set; }
    }

    public class CreatePropertyTypeCommandHandler : IRequestHandler<CreatePropertyTypeCommand, int>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public CreatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertytype = _mapper.Map<PropertyType>(command);
            propertytype = await _propertyTypeRepository.AddAsync(propertytype);  
            return propertytype.Id;
        }
    }
}
