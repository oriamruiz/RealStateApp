using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Enums;
using RealStateApp.Core.Application.Features.Properties.Queries.GetProperty;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Domain.Entities;


namespace RealStateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes
{
    public class GetAllSaleTypesQuery : IRequest<IList<SaleTypeDto>>
    {

    }

    public class GetAllSaleTypesQueryHandler : IRequestHandler<GetAllSaleTypesQuery, IList<SaleTypeDto>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetAllSaleTypesQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<IList<SaleTypeDto>> Handle(GetAllSaleTypesQuery request, CancellationToken cancellationToken)
        {
            var saletypes = await _saleTypeRepository.GetAllAsync();

            if (saletypes == null || saletypes.Count == 0) throw new Exception("sale types not found");

            var saletypesdto = _mapper.Map<List<SaleTypeDto>>(saletypes);

            return saletypesdto;
        }


    }
}
