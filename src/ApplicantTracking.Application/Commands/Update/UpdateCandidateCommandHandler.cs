using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Domain.Events;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Commands.Update;

public sealed class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;

    public UpdateCandidateCommandHandler(IUnitOfWork uow, IMediator mediator)
    {
        _uow = uow;
        _mediator = mediator;
    }

    public async Task<bool> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _uow.Candidates.GetByIdAsync(request.IdCandidate, cancellationToken);
        if (candidate is null) return false;

        var oldData = JsonSerializer.Serialize(candidate);

        candidate.Update(
            request.Candidate.Name,
            request.Candidate.Surename,
            request.Candidate.Birthdate,
            request.Candidate.Email);

        await using var tx = await _uow.BeginTransactionAsync(cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        var newData = JsonSerializer.Serialize(candidate);
        await _mediator.Publish(
            new CandidateUpdatedEvent(
                IdAggregateRoot: candidate.IdCandidate,
                OldData: oldData,
                NewData: newData,
                OccurredAtUtc: System.DateTime.UtcNow),
            cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return true;
    }
}

