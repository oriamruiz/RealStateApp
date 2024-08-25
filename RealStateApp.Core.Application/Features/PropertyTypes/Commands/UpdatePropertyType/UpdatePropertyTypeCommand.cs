using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;


namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType
{
    /// <summary>
    /// Parametros para la actualizacion de un tipo de propiedad
    /// </summary>
    public class UpdatePropertyTypeCommand : IRequest<PropertyTypeUpdateResponse>
    {

        [SwaggerParameter(Description = "id del tipo de propiedad a editar")]
        public int Id { get; set; }

        [SwaggerParameter(Description = "nuevo nombre del tipo de propiedad ")]
        public string Name { get; set; }

        [SwaggerParameter(Description = "nueva descripcion del tipo de propiedad ")]
        public string Description { get; set; }
    }

    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, PropertyTypeUpdateResponse>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<PropertyTypeUpdateResponse> Handle(UpdatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertytypetoupdate = await _propertyTypeRepository.GetByIdAsync(command.Id);

            if(propertytypetoupdate == null) throw new Exception("property type not found");
         
            propertytypetoupdate = _mapper.Map<PropertyType>(command);

            await _propertyTypeRepository.UpdateAsync(propertytypetoupdate, propertytypetoupdate.Id);

            var propertytypetoupdateresponse = _mapper.Map<PropertyTypeUpdateResponse>(propertytypetoupdate);

            return propertytypetoupdateresponse;
        }
    }
}
