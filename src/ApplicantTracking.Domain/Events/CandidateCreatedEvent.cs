using System;
using MediatR;

namespace ApplicantTracking.Domain.Events;

public sealed record CandidateCreatedEvent(
    int IdAggregateRoot,
    string? OldData,
    string? NewData,
    DateTime OccurredAtUtc) : INotification;

