using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypesQuery;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements
{
    public class GetAllImprovementsQuery : IRequest<IList<ImprovementDto>>
    {

    }

    public class GetAllImprovementsQueryHandler : IRequestHandler<GetAllImprovementsQuery, IList<ImprovementDto>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetAllImprovementsQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<IList<ImprovementDto>> Handle(GetAllImprovementsQuery request, CancellationToken cancellationToken)
        {
            var improvements = await _improvementRepository.GetAllAsync();

            if (improvements == null || improvements.Count == 0) throw new Exception("Improvements not found");

            var improvementsdto = _mapper.Map<List<ImprovementDto>>(improvements);

            return improvementsdto;
        }


    }
}
