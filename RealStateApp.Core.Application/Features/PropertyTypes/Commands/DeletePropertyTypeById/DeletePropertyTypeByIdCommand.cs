using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById
{
    /// <summary>
    /// Parametros para eliminar un tipo de propiedad
    /// </summary>
    public class DeletePropertyTypeByIdCommand : IRequest<IList<int>>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "Id del tipo de propiedad a borrar")]
        public int Id { get; set; }
    }

    public class DeletePropertyTypeByIdCommandHandler : IRequestHandler<DeletePropertyTypeByIdCommand, IList<int>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;

        public DeletePropertyTypeByIdCommandHandler(IPropertyTypeRepository propertyTypeRepository, IPropertyRepository propertyRepository, IPropertyImprovementRepository propertyImprovementRepository, IFavoritePropertyRepository favoritePropertyRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _propertyRepository = propertyRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
            _favoritePropertyRepository = favoritePropertyRepository;
        }
        public async Task<IList<int>> Handle(DeletePropertyTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var propertytype = await _propertyTypeRepository.GetByIdAsync(command.Id);

            if (propertytype == null) throw new Exception("property type not found");

            var propertiesremoved = await DeleteAllPropertiesWithThisType(propertytype.Id);

            await _propertyTypeRepository.DeleteAsync(propertytype);

            return propertiesremoved;

        }

        private async Task<IList<int>> DeleteAllPropertiesWithThisType(int id)
        {
            List<int> result = null;

            var properties = await _propertyRepository.GetAllAsync();

            var propertiestodelete = properties.Where(p => p.PropertyTypeId == id).ToList();

            if(propertiestodelete !=  null && propertiestodelete.Count > 0)
            {
                result = propertiestodelete.Select(p=> p.Id).ToList();

                foreach (var propertytodelete in propertiestodelete)
                {

                    var improvements = await _propertyImprovementRepository.GetAllAsync();

                    var propertyimprovements = improvements.Where(pi => pi.PropertyId == propertytodelete.Id).ToList();

                    if (propertyimprovements != null && propertyimprovements.Count > 0)
                    {
                        foreach (var propimp in propertyimprovements)
                        {
                            await _propertyImprovementRepository.DeleteAsync(propimp);
                        }
                    }

                    var favoriteProperties = await _favoritePropertyRepository.GetAllAsync();

                    var favoritePropertiesToDelete = favoriteProperties.Where(f => f.PropertyId == propertytodelete.Id).ToList();

                    if (favoritePropertiesToDelete != null && favoritePropertiesToDelete.Count > 0)
                    {
                        foreach (var fav in favoritePropertiesToDelete)
                        {
                            await _favoritePropertyRepository.DeleteAsync(fav);
                        }
                    }

                    await _propertyRepository.DeleteAsync(propertytodelete);

                }


            }


            return result;

        }
    }
}
