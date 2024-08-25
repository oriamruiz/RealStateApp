using MediatR;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById
{
    /// <summary>
    /// Parametros para eliminar una mejora
    /// </summary>
    public class DeleteImprovementByIdCommand : IRequest<int>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "Id de la mejora a eliminar")]
        public int Id { get; set; }
    }

    public class DeleteImprovementByIdCommandHandler : IRequestHandler<DeleteImprovementByIdCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;

        public DeleteImprovementByIdCommandHandler(IImprovementRepository propertyTypeRepository, IPropertyRepository propertyRepository, IPropertyImprovementRepository propertyImprovementRepository, IFavoritePropertyRepository favoritePropertyRepository)
        {
            _improvementRepository = propertyTypeRepository;
            _propertyImprovementRepository = propertyImprovementRepository;
        }
        public async Task<int> Handle(DeleteImprovementByIdCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id);

            if (improvement == null) throw new Exception("Improvement not found");

            await RemoveRelationshipPropertiesWithThisImprovement(improvement.Id);

            await _improvementRepository.DeleteAsync(improvement);

            return improvement.Id;

        }

        private async Task RemoveRelationshipPropertiesWithThisImprovement(int id)
        {

            var propertiesrelaionship = await _propertyImprovementRepository.GetAllAsync();

            if(propertiesrelaionship != null && propertiesrelaionship.Count > 0)
            {
                var propertiesrelaionshiptoremove = propertiesrelaionship.Where(pr => pr.ImprovementId == id).ToList();

                if(propertiesrelaionshiptoremove != null || propertiesrelaionshiptoremove.Count > 0)
                {
                    foreach(var pr in propertiesrelaionshiptoremove)
                    {
                        await _propertyImprovementRepository.DeleteAsync(pr);    
                    }
                
                
                }


            }


        }
    }
}
