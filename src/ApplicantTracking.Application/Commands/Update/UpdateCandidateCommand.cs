using ApplicantTracking.Application.DTOs;
using MediatR;

namespace ApplicantTracking.Application.Commands.Update;

public sealed record UpdateCandidateCommand(int IdCandidate, UpdateCandidateDto Candidate) : IRequest<bool>;

