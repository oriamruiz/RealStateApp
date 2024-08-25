using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement
{
    /// <summary>
    /// Parametros para la creacion de una mejora
    /// </summary>
    public class CreateImprovementCommand : IRequest<int>
    {
        /// <example>
        /// Piscina
        /// </example>
        [SwaggerParameter(Description = "nombre de la mejora")]
        public string Name { get; set; }
        /// <example>
        /// La mejora "Piscina" consiste en la instalación de una piscina en la propiedad....
        /// </example>
        [SwaggerParameter(Description = "descripcion de la mejora")]
        public string Description { get; set; }
    }

    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvement = _mapper.Map<Improvement>(command);
            improvement = await _improvementRepository.AddAsync(improvement);
            return improvement.Id;
        }
    }
}
