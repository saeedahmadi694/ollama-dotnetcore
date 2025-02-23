using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.BackgroundTasks.BackgroundServices;

public class SampleBackgroundService : ScheduledBackgroundService
{
    public SampleBackgroundService(ILogger logger, IServiceProvider services, IMediator mediator) : base(logger, services, false,false)
    {
    }

    protected override async Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        Console.WriteLine("Testing job");

    }

    protected override string GetCronExpression()
    {
        return "* */1 * * *";
    }
}



