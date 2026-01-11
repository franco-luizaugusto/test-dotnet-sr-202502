using ApplicantTracking.Application.DTOs;
using MediatR;

namespace ApplicantTracking.Application.Commands.Create;

public sealed record CreateCandidateCommand(CreateCandidateDto Candidate) : IRequest<CandidateDto>;

