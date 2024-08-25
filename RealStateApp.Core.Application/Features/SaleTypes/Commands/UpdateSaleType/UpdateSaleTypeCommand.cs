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

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{
    /// <summary>
    /// Parametros para la actualizacion de un tipo de venta
    /// </summary>
    public class UpdateSaleTypeCommand : IRequest<SaleTypeUpdateResponse>
    {
        [SwaggerParameter(Description = "id del tipo de venta a editar")]
        public int Id { get; set; }

        [SwaggerParameter(Description = "nuevo nombre del tipo de venta ")]
        public string Name { get; set; }

        [SwaggerParameter(Description = "nueva descripcion del tipo de venta ")]
        public string Description { get; set; }
    }

    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, SaleTypeUpdateResponse>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public UpdateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<SaleTypeUpdateResponse> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saletypetoupdate = await _saleTypeRepository.GetByIdAsync(command.Id);

            if (saletypetoupdate == null) throw new Exception("sale type not found");

            saletypetoupdate = _mapper.Map<SaleType>(command);

            await _saleTypeRepository.UpdateAsync(saletypetoupdate, saletypetoupdate.Id);

            var saletypetoupdateresponse = _mapper.Map<SaleTypeUpdateResponse>(saletypetoupdate);

            return saletypetoupdateresponse;
        }

       
    }
}
