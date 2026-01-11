using System;

namespace ApplicantTracking.Application.DTOs;

public sealed class UpdateCandidateDto
{
    public string Name { get; set; } = null!;
    public string Surename { get; set; } = null!;
    public DateTime Birthdate { get; set; }
    public string Email { get; set; } = null!;
}

