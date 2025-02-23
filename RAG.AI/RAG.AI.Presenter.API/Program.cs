using RAG.AI.Application.IntegrationEvents;
using RAG.AI.Infrastructure.Persistent;
using RAG.AI.Infrastructure.Persistent.DataSeeding;
using RAG.AI.Presenter.API.DependencyInjection;
using RAG.AI.Presenter.API.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Parbad.Storage.EntityFrameworkCore.Context;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "CorsPolicy";

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment env = builder.Environment;

#region Configure Services

builder.Host.UseSerilog((hostBuilderContext, LoggerConfiguration) =>
{
    LoggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
    .Filter.ByExcluding(c => FilterLog(c.Properties));
});

builder.Services.AddScoped(typeof(IntegrationEventStore));
builder.Services.AddScoped(typeof(MediatrIntegrationEventStore));

builder.Services.AddDbContext<AIContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("RAG.AI.Infrastructure")));

builder.Services.AddDbContext<AIReadOnlyContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Readonly")));

builder.Services.AddCors(configuration, env, CorsPolicy);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddConfiguredMediatR();
builder.Services.AddConfiguredMinio(configuration);
builder.Services.AddAppConfigurations(configuration);
builder.Services.AddConfiguredStackExchangeRedisCache(configuration);
builder.Services.AddOperationLockManager();
builder.Services.AddTokenAuthentication(configuration);
builder.Services.AddRepositories(configuration);

builder.Services.AddScoped<IAIIntegrationEventService, AIIntegrationEventService>();
builder.Services.AddScoped<IAIMediatrIntegrationEventService, AIMediatrIntegrationEventService>();

builder.Services.AddConfiguredHostedServices(configuration);


builder.Services.AddControllers(options => { options.Filters.Add<UnhandledExceptionFilterAttribute>(); });
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<DataSeeder>();

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "RAG.Api", Version = "v1" });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

//builder.Services.AddTokenAuthentication(configuration);
var app = builder.Build();


#endregion


#region Pipelines

// Configure the HTTP request pipeline.
var swaggerHost = configuration["Swagger:Host"];

//if (!(app.Environment.EnvironmentName == "Local"))
//{
//    var basePath = "/awards";
//    app.UseSwagger(c =>
//    {
//        c.RouteTemplate = "swagger/{documentName}/swagger.json";
//        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
//        {
//            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{swaggerHost}{basePath}" } };
//        });
//    });

//}
Console.WriteLine($"Current running environment is: {app.Environment.EnvironmentName}");

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("endpointMethod",
            httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.DisplayName);
    };
});

//app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.UseCors(CorsPolicy);

if (!env.IsProduction())
{
    app.UseParbadVirtualGateway();
}

app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "V1"));

//if (!app.Environment.IsProduction())
//{
//    app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");

//    app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "V1"));

//}

//ADD AFTER CONTEXT HAS DBSETS
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AIContext>();
    db.Database.Migrate();


    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedData();
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

#endregion


static bool FilterLog(IReadOnlyDictionary<string, Serilog.Events.LogEventPropertyValue> logProperties)
{
    if (logProperties.ContainsKey("StatusCode") && logProperties.ContainsKey("RequestMethod"))
    {
        //ignore successful GET requests
        if (logProperties["StatusCode"].ToString().Equals("200") && logProperties["RequestMethod"].ToString().Contains("GET"))
        {
            return true;
        }
    }
    return false;
}


