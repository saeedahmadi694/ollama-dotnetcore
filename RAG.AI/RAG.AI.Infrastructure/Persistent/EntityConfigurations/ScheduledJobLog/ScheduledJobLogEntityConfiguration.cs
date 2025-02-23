using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;
using RAG.AI.Infrastructure.Extentions;
namespace RAG.AI.Infrastructure.Persistent.EntityConfigurations.ScheduledJobLog
{
    public class ScheduledJobLogEntityConfiguration : IEntityTypeConfiguration<Domain.Aggregates.ScheduledJobLogAggregate.ScheduledJobLog>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Aggregates.ScheduledJobLogAggregate.ScheduledJobLog> builder)
        {
            builder.ToTable("ScheduledJobLogs", AIContext.DEFAULT_SCHEMA);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.JobName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.Status)
                .HasDefaultValue(ScheduledJobLogStatus.InProgress);
            builder.OwnEnumeration(e => e.Status);
        }
    }
}

