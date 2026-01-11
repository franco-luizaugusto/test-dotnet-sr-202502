using ApplicantTracking.Application.DTOs;
using FluentValidation;
using System;

namespace ApplicantTracking.Application.Commands.Update;

// Validates the input DTO used by the UpdateCandidate command (kept as DTO validation to integrate with ASP.NET auto-validation).
public sealed class UpdateCandidateCommandValidator : AbstractValidator<UpdateCandidateDto>
{
    public UpdateCandidateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(x => x.Surename)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(250)
            .EmailAddress();

        RuleFor(x => x.Birthdate)
            .Must(d => d.Date <= DateTime.UtcNow.Date)
            .WithMessage("Birthdate cannot be in the future.");
    }
}

