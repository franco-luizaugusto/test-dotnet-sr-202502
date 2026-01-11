using ApplicantTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantTracking.Infrastructure.Data.Configurations;

public sealed class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("candidates", "dbo");

        builder.HasKey(x => x.IdCandidate);
        builder.Property(x => x.IdCandidate).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Surename).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(250);

        builder.Property(x => x.Birthdate).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.LastUpdatedAt).IsRequired(false);
    }
}

