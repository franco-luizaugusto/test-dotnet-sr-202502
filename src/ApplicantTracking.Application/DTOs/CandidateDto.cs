using System;

namespace ApplicantTracking.Application.DTOs;

public sealed class CandidateDto
{
    public int IdCandidate { get; set; }
    public string Name { get; set; } = null!;
    public string Surename { get; set; } = null!;
    public DateTime Birthdate { get; set; }
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
}

