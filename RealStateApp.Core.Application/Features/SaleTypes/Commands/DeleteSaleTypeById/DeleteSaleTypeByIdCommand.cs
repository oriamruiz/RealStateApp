using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById
{
    /// <summary>
    /// Parametros para eliminar un tipo de venta
    /// </summary>
    public class DeleteSaleTypeByIdCommand : IRequest<IList<int>>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "Id del tipo de venta a borrar")]
        public int Id { get; set; }
    }

    public class DeleteSaleTypeByIdCommandHandler : IRequestHandler<DeleteSaleTypeByIdCommand, IList<int>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IFavoritePropertyRepository _favoritePropertyRepository;

        public DeleteSaleTypeByIdCommandHandler(ISaleTypeRepository saleTypeRepository, IPropertyRepository propertyRepository, IPropertyImprovementRepository propertyImprovementRepository, IFavoritePropertyRepository favoritePropertyRepository)
        {
            _saleTypeRepository = saleTypeRepository;
            _propertyRepository = propertyRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
            _favoritePropertyRepository = favoritePropertyRepository;
        }
        public async Task<IList<int>> Handle(DeleteSaleTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var saletype = await _saleTypeRepository.GetByIdAsync(command.Id);

            if (saletype == null) throw new Exception("sale type not found");

            var propertiesremoved = await DeleteAllPropertiesWithThisType(saletype.Id);

            await _saleTypeRepository.DeleteAsync(saletype);

            return propertiesremoved;

        }

        private async Task<IList<int>> DeleteAllPropertiesWithThisType(int id)
        {
            List<int> result = null;

            var properties = await _propertyRepository.GetAllAsync();

            var propertiestodelete = properties.Where(p => p.SaleTypeId == id).ToList();

            if (propertiestodelete != null && propertiestodelete.Count > 0)
            {
                result = propertiestodelete.Select(p => p.Id).ToList();

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
