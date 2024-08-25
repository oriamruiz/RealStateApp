using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    /// <summary>
    /// Parametros para la actualizacion de una mejora
    /// </summary>
    public class UpdateImprovementCommand : IRequest<ImprovementUpdateResponse>
    {
        
        [SwaggerParameter(Description = "id de la mejora a editar")]
        public int Id { get; set; }
       
        [SwaggerParameter(Description = "nuevo nombre de la mejora")]
        public string Name { get; set; }
        
        [SwaggerParameter(Description = "nueva descripcion de la mejora")]
        public string Description { get; set; }
    }

    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, ImprovementUpdateResponse>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<ImprovementUpdateResponse> Handle(UpdateImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvementtoupdate = await _improvementRepository.GetByIdAsync(command.Id);

            if (improvementtoupdate == null) throw new Exception("Improvement not found");

            improvementtoupdate = _mapper.Map<Improvement>(command);

            await _improvementRepository.UpdateAsync(improvementtoupdate, improvementtoupdate.Id);

            var improvementtoupdateresponse = _mapper.Map<ImprovementUpdateResponse>(improvementtoupdate);

            return improvementtoupdateresponse;
        }
    }
}
