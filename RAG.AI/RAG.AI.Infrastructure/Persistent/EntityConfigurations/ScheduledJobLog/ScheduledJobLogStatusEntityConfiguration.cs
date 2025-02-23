using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;

namespace RAG.AI.Infrastructure.Persistent.EntityConfigurations.ScheduledJobLog;

public class ScheduledJobLogStatusEntityConfiguration : IEntityTypeConfiguration<ScheduledJobLogStatus>
{
    public void Configure(EntityTypeBuilder<ScheduledJobLogStatus> builder)
    {
        builder.ToTable("ScheduledJobLogStatuses", AIContext.DEFAULT_SCHEMA);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(o => o.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}

