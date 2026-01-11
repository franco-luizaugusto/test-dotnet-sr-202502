using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Domain.Events;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.Commands.Delete;

public sealed class DeleteCandidateCommandHandler : IRequestHandler<DeleteCandidateCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;

    public DeleteCandidateCommandHandler(IUnitOfWork uow, IMediator mediator)
    {
        _uow = uow;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _uow.Candidates.GetByIdAsync(request.IdCandidate, cancellationToken);
        if (candidate is null) return false;

        var oldData = JsonSerializer.Serialize(candidate);

        _uow.Candidates.Remove(candidate);

        await using var tx = await _uow.BeginTransactionAsync(cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(
            new CandidateDeletedEvent(
                IdAggregateRoot: request.IdCandidate,
                OldData: oldData,
                NewData: null,
                OccurredAtUtc: System.DateTime.UtcNow),
            cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return true;
    }
}

