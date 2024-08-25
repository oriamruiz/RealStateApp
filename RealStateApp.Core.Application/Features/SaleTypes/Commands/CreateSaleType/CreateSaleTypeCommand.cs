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

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType
{
    /// <summary>
    /// Parametros para la creacion de un tipo de venta
    /// </summary>
     public class CreateSaleTypeCommand : IRequest<int>
    {
        /// <example>
        /// Alquiler
        /// </example>
        [SwaggerParameter(Description = "nombre del tipo de venta")]
        public string Name { get; set; }
        /// <example>
        /// Propiedad disponible para arrendamiento, ideal para.....
        /// </example>
        [SwaggerParameter(Description = "descripcion del tipo de venta")]
        public string Description { get; set; }
    }

    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public CreateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saletype = _mapper.Map<SaleType>(command);
            saletype = await _saleTypeRepository.AddAsync(saletype);
            return saletype.Id;
        }
    }
}
