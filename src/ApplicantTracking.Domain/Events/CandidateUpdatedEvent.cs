using System;
using MediatR;

namespace ApplicantTracking.Domain.Events;

public sealed record CandidateUpdatedEvent(
    int IdAggregateRoot,
    string? OldData,
    string? NewData,
    DateTime OccurredAtUtc) : INotification;

