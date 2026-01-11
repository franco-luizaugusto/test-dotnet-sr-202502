using ApplicantTracking.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace ApplicantTracking.Tests.Domain;

public sealed class CandidateTests
{
    [Fact]
    public void Ctor_ShouldNormalizeEmailToLowercase()
    {
        var candidate = new Candidate("John", "Doe", new DateTime(1990, 1, 1), "  TeSt@Example.Com  ");
        candidate.Email.Should().Be("test@example.com");
    }

    [Fact]
    public void Update_ShouldNormalizeEmailToLowercase()
    {
        var candidate = new Candidate("John", "Doe", new DateTime(1990, 1, 1), "a@b.com");
        candidate.Update("John", "Doe", new DateTime(1990, 1, 1), "  X@Y.COM ");
        candidate.Email.Should().Be("x@y.com");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenBirthdateIsInFuture()
    {
        var future = DateTime.UtcNow.Date.AddDays(1);
        Action act = () => new Candidate("John", "Doe", future, "a@b.com");
        act.Should().Throw<ArgumentException>().WithMessage("*Birthdate*");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenNameIsEmpty()
    {
        Action act = () => new Candidate("", "Doe", new DateTime(1990, 1, 1), "a@b.com");
        act.Should().Throw<ArgumentException>().WithMessage("*Name*");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenSurenameIsEmpty()
    {
        Action act = () => new Candidate("John", "", new DateTime(1990, 1, 1), "a@b.com");
        act.Should().Throw<ArgumentException>().WithMessage("*Surename*");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenNameTooLong()
    {
        var name = new string('a', 81);
        Action act = () => new Candidate(name, "Doe", new DateTime(1990, 1, 1), "a@b.com");
        act.Should().Throw<ArgumentException>().WithMessage("*max length is 80*");
    }

    [Fact]
    public void Ctor_ShouldThrow_WhenSurenameTooLong()
    {
        var surename = new string('a', 151);
        Action act = () => new Candidate("John", surename, new DateTime(1990, 1, 1), "a@b.com");
        act.Should().Throw<ArgumentException>().WithMessage("*max length is 150*");
    }
}

