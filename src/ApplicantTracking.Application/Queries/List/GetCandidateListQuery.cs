using ApplicantTracking.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace ApplicantTracking.Application.Queries.List;

public sealed record GetCandidateListQuery() : IRequest<List<CandidateDto>>;

