using ApplicantTracking.Application.DTOs;
using MediatR;

namespace ApplicantTracking.Application.Queries.GetById;

public sealed record GetCandidateByIdQuery(int IdCandidate) : IRequest<CandidateDto?>;

