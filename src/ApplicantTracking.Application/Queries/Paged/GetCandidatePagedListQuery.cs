using ApplicantTracking.Application.DTOs;
using MediatR;

namespace ApplicantTracking.Application.Queries.Paged;

public sealed record GetCandidatePagedListQuery(int Page, int ItemsPerPage) : IRequest<PagedList<CandidateDto>>;

