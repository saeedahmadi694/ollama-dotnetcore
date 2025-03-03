using Microsoft.Extensions.DependencyInjection;
using RAG.AI.Application.Commands.Imports.ProcessImportJob;
using RAG.AI.Domain.Aggregates.ImportAggregate;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;
using Serilog;

namespace RAG.AI.BackgroundTasks.BackgroundServices;
public class ProccessCreatedJobBackgroundService : ScheduledBackgroundService
{
    private readonly IImportJobQueryService _importJobQueryService;
    public ProccessCreatedJobBackgroundService(ILogger logger, IServiceProvider services, IImportJobQueryService importJobQueryService) : base(logger, services, false, false)
    {
        _importJobQueryService = importJobQueryService;
    }

    protected override async Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var jobs = await _importJobQueryService.GetAllAsync(r => r.Status == ImportJobStatus.Created);
        foreach (var job in jobs)
        {
            await _mediator.Send(new ProcessImportJobCommand(job.Id));
        }
    }
    protected override string GetCronExpression()
    {
        return "* * * * *";
    }
}

