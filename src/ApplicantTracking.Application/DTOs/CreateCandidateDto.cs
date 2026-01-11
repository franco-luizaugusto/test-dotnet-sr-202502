using System;

namespace ApplicantTracking.Application.DTOs;

public sealed class CreateCandidateDto
{
    public string Name { get; set; } = null!;
    public string Surename { get; set; } = null!;
    public DateTime Birthdate { get; set; }
    public string Email { get; set; } = null!;
}

