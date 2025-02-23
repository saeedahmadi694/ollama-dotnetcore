using RAG.AI.Application.IntegrationEvents;
using RAG.AI.Infrastructure.Persistent.DataSeeding;
using RAG.AI.Presenter.Web.DependencyInjection;
using RAG.AI.Presenter.Web.Filters;
using Microsoft.AspNetCore.Http.Features;
using Parbad.Storage.EntityFrameworkCore.Context;
using HttpRequest = RAG.AI.Infrastructure.Utilities.HttpRequest;

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

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddConfiguredMediatR();
builder.Services.AddConfiguredMassTransit(configuration);
builder.Services.AddConfiguredMinio(configuration);
builder.Services.AddAppConfigurations(configuration);
builder.Services.AddConfiguredStackExchangeRedisCache(configuration);
builder.Services.AddOperationLockManager();
builder.Services.AddConfiguredHostedServices(configuration);
builder.Services.AddConfiguredCookieAuthentication();

builder.Services.AddRepositories(configuration);
builder.Services.AddScoped<IHttpRequest, HttpRequest>();
builder.Services.AddScoped<IAIIntegrationEventService, AIIntegrationEventService>();
builder.Services.AddScoped<IAIMediatrIntegrationEventService, AIMediatrIntegrationEventService>();
builder.Services.AddScoped<DataSeeder>();


builder.Services.AddControllersWithViews(options => { options.Filters.Add<UnhandledExceptionFilterAttribute>(); });


var app = builder.Build();


#endregion

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseCors(CorsPolicy);


app.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");



//ADD AFTER CONTEXT HAS DBSETS
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AIContext>();
    db.Database.Migrate();


    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedData();
}

app.Run();



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



