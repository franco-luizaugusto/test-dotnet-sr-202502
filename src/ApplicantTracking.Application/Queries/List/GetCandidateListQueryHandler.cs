using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.DTOs;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Queries.List;

public sealed class GetCandidateListQueryHandler : IRequestHandler<GetCandidateListQuery, List<CandidateDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetCandidateListQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<List<CandidateDto>> Handle(GetCandidateListQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _uow.Candidates.ListAsync(cancellationToken);
        return _mapper.Map<List<CandidateDto>>(candidates);
    }
}

