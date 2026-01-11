using System;

namespace ApplicantTracking.Domain.Entities;

public sealed class Timeline
{
    private Timeline() { } // EF

    public Timeline(int idAggregateRoot, byte idTimelineType, string? oldData, string? newData)
    {
        if (idAggregateRoot <= 0) throw new ArgumentOutOfRangeException(nameof(idAggregateRoot));

        IdAggregateRoot = idAggregateRoot;
        IdTimelineType = idTimelineType;
        OldData = oldData;
        NewData = newData;
        CreatedAt = DateTime.UtcNow;
    }

    public int IdTimeline { get; private set; }
    public int IdAggregateRoot { get; private set; }
    public byte IdTimelineType { get; private set; }
    public string? OldData { get; private set; }
    public string? NewData { get; private set; }
    public DateTime CreatedAt { get; private set; }
}

