using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Enumerators;
using ApplicantTracking.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.EventHandlers;

public sealed class CandidateCreatedEventHandler : INotificationHandler<CandidateCreatedEvent>
{
    private readonly IUnitOfWork _uow;

    public CandidateCreatedEventHandler(IUnitOfWork uow) => _uow = uow;

    public Task Handle(CandidateCreatedEvent notification, CancellationToken cancellationToken) =>
        _uow.Timelines.AddAsync(
            new Timeline(
                idAggregateRoot: notification.IdAggregateRoot,
                idTimelineType: (byte)TimelineTypes.Create,
                oldData: notification.OldData,
                newData: notification.NewData),
            cancellationToken);
}

