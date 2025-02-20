using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddDbContext<RagDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IVectorStore, VectorStore>();

// Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Hangfire with PostgreSQL
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
    {
        QueuePollInterval = TimeSpan.FromSeconds(15),
        JobExpirationCheckInterval = TimeSpan.FromHours(1),
        CountersAggregateInterval = TimeSpan.FromMinutes(5),
        PrepareSchemaIfNecessary = true,
        SchemaName = "hangfire"
    }));

// Add Hangfire server
builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 2;
    options.Queues = new[] { "default", "documents", "embeddings" };
});

// Register the document processing service
builder.Services.AddScoped<IDocumentProcessingService, DocumentProcessingService>();

// Add Ollama Embedding Service
builder.Services.AddSingleton<IEmbeddingService, OllamaEmbeddingService>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<OllamaHealthCheck>("ollama_service");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Configure Hangfire dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Document Processing Jobs",
    // Optional: Add authorization
    Authorization = new[] 
    { 
        new HangfireCustomAuthorizationFilter() 
    }
});

app.Run();
