using System;

namespace ApplicantTracking.Domain.Entities;

public sealed class Candidate
{
    private Candidate() { } // EF

    public Candidate(string name, string surename, DateTime birthdate, string email)
    {
        SetName(name);
        SetSurename(surename);
        SetBirthdate(birthdate);
        SetEmail(email);

        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = null;
    }

    public int IdCandidate { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surename { get; private set; } = null!;
    public DateTime Birthdate { get; private set; }
    public string Email { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }

    public void Update(string name, string surename, DateTime birthdate, string email)
    {
        SetName(name);
        SetSurename(surename);
        SetBirthdate(birthdate);
        SetEmail(email);

        LastUpdatedAt = DateTime.UtcNow;
    }

    private void SetName(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name is required.", nameof(value));
        if (value.Length > 80) throw new ArgumentException("Name max length is 80.", nameof(value));
        Name = value.Trim();
    }

    private void SetSurename(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Surename is required.", nameof(value));
        if (value.Length > 150) throw new ArgumentException("Surename max length is 150.", nameof(value));
        Surename = value.Trim();
    }

    private void SetBirthdate(DateTime value)
    {
        // Keep it simple: just ensure it's not a future date.
        if (value.Date > DateTime.UtcNow.Date) throw new ArgumentException("Birthdate cannot be in the future.", nameof(value));
        Birthdate = value;
    }

    private void SetEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email is required.", nameof(value));
        if (value.Length > 250) throw new ArgumentException("Email max length is 250.", nameof(value));
        Email = value.Trim().ToLowerInvariant();
    }
}

