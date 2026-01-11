using ApplicantTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicantTracking.Infrastructure.Data.Configurations;

public sealed class TimelineConfiguration : IEntityTypeConfiguration<Timeline>
{
    public void Configure(EntityTypeBuilder<Timeline> builder)
    {
        builder.ToTable("timelines", "dbo");

        builder.HasKey(x => x.IdTimeline);
        builder.Property(x => x.IdTimeline).ValueGeneratedOnAdd();

        builder.Property(x => x.IdAggregateRoot).IsRequired();
        builder.Property(x => x.IdTimelineType).IsRequired();

        builder.Property(x => x.OldData).HasColumnType("varchar(max)").IsRequired(false);
        builder.Property(x => x.NewData).HasColumnType("varchar(max)").IsRequired(false);

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}

