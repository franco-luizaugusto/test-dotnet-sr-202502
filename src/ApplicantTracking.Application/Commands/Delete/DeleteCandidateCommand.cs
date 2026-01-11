using MediatR;

namespace ApplicantTracking.Application.Commands.Delete;

public sealed record DeleteCandidateCommand(int IdCandidate) : IRequest<bool>;

