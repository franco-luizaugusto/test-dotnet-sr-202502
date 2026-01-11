using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Application.DTOs;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Queries.Paged;

public sealed class GetCandidatePagedListQueryHandler : IRequestHandler<GetCandidatePagedListQuery, PagedList<CandidateDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetCandidatePagedListQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedList<CandidateDto>> Handle(GetCandidatePagedListQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _uow.Candidates.ListPagedAsync(request.Page, request.ItemsPerPage, cancellationToken);
        var dtoItems = _mapper.Map<List<CandidateDto>>(items);
        return new PagedList<CandidateDto>(dtoItems, totalCount, request.Page, request.ItemsPerPage);
    }
}

