using System;
using MediatR;

namespace ApplicantTracking.Domain.Events;

public sealed record CandidateDeletedEvent(
    int IdAggregateRoot,
    string? OldData,
    string? NewData,
    DateTime OccurredAtUtc) : INotification;

