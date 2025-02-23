using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Pgvector;
public class RagDbContext : DbContext
{
    public DbSet<DocumentEntity> Documents { get; set; }

    public RagDbContext(DbContextOptions<RagDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connString = "Host=localhost;Database=RAG_Db;Username=postgres;Password=saeedahmadi694";
        optionsBuilder.UseNpgsql(connString, o => o.UseVector());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<DocumentEntity>(entity =>
        {
            entity.ToTable("Documents");

            //entity.HasKey(e => e.Id);
            //entity.Property(e => e.Id).HasColumnName("id");
            //entity.Property(e => e.Content).HasColumnName("content").IsRequired();
            //entity.Property(e => e.Title).HasColumnName("title");
            //entity.Property(e => e.CreatedAt)
            //    .HasColumnName("created_at")
            //    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.Embedding)
                .HasColumnName("Embedding")
                .HasColumnType("vector(1536)");

            entity.Property(e => e.Metadata)
                .HasColumnName("MetaData")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null)
                );
        });
    }
}

public class DocumentEntity
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }

    [Column(TypeName = "vector(1536)")]
    public Vector Embedding { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
} 