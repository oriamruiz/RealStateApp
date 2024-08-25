using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById
{
    /// <summary>
    /// parametros para filtrar tipo de producto
    /// </summary>
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeDto>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "id del tipo de producto a buscar")]
        public int Id { get; set; }
    }

    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeDto>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetSaleTypeByIdQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<SaleTypeDto> Handle(GetSaleTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var saletype = await _saleTypeRepository.GetByIdAsync(request.Id);

            if (saletype == null) throw new Exception("sale type not found");

            var saletypedto = _mapper.Map<SaleTypeDto>(saletype);

            return saletypedto;
        }
    }



}
