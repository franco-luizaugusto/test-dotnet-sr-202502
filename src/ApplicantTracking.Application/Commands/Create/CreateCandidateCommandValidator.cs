using ApplicantTracking.Application.DTOs;
using FluentValidation;
using System;

namespace ApplicantTracking.Application.Commands.Create;

public sealed class CreateCandidateCommandValidator : AbstractValidator<CreateCandidateDto>
{
    public CreateCandidateCommandValidator()
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

