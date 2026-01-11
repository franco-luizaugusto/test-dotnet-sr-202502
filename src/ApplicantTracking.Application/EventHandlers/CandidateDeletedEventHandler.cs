using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Domain.Entities;
using ApplicantTracking.Domain.Enumerators;
using ApplicantTracking.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantTracking.Application.EventHandlers;

public sealed class CandidateDeletedEventHandler : INotificationHandler<CandidateDeletedEvent>
{
    private readonly IUnitOfWork _uow;

    public CandidateDeletedEventHandler(IUnitOfWork uow) => _uow = uow;

    public Task Handle(CandidateDeletedEvent notification, CancellationToken cancellationToken) =>
        _uow.Timelines.AddAsync(
            new Timeline(
                idAggregateRoot: notification.IdAggregateRoot,
                idTimelineType: (byte)TimelineTypes.Delete,
                oldData: notification.OldData,
                newData: notification.NewData),
            cancellationToken);
}

