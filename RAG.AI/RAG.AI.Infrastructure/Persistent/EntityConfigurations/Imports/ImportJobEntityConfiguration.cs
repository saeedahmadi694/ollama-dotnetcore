using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAG.AI.Domain.Aggregates.ImportAggregate;

namespace RAG.AI.Infrastructure.Persistent.EntityConfigurations.Imports;
public class ImportJobEntityConfiguration : IEntityTypeConfiguration<ImportJob>
{
    public void Configure(EntityTypeBuilder<ImportJob> builder)
    {
        builder.ToTable("ImportJobs");
        builder.HasKey(t => t.Id);
        builder.Ignore(e => e.DomainEvents);
        builder.Property(e => e.Id)
            .UseHiLo("ImportJobSeq");


        builder.Property(e => e.FileAddress)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.FileName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion(e => e.Id,
                           value => Enumeration.FromValue<ImportJobStatus>(value));

    }
}
