using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeByIdQuery;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;


namespace RealStateApp.Core.Application.Features.Improvements.Queries.GetImprovementById
{
    /// <summary>
    /// Parametros para la busqueda de mejora
    /// </summary>
    public class GetImprovementByIdQuery : IRequest<ImprovementDto>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "id de la mejora a buscar")]
        public int Id { get; set; }
    }

    public class GetImprovementsByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, ImprovementDto>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetImprovementsByIdQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<ImprovementDto> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(request.Id);

            if (improvement == null) throw new Exception("Improvement type not found");

            var improvementdto = _mapper.Map<ImprovementDto>(improvement);

            return improvementdto;
        }
    }
}
