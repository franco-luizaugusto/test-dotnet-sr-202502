using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.DTOs;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Queries.GetById;

public sealed class GetCandidateByIdQueryHandler : IRequestHandler<GetCandidateByIdQuery, CandidateDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetCandidateByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<CandidateDto?> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _uow.Candidates.GetByIdAsync(request.IdCandidate, cancellationToken);
        return candidate is null ? null : _mapper.Map<CandidateDto>(candidate);
    }
}

