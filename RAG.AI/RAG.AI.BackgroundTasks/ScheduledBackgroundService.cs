using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
using RAG.AI.Application.Commands.ScheduledJobLogs.CreateFinishJobLog;
using RAG.AI.Application.Commands.ScheduledJobLogs.CreateNewScheduledJobLog;
using Serilog;

namespace RAG.AI.BackgroundTasks;

public abstract class ScheduledBackgroundService : IHostedService, IDisposable
{
    protected readonly ILogger _logger;
    //protected readonly IOperationLockManager _lockManager;
    protected IServiceProvider _services;
    protected CrontabSchedule _cronSchedule;
    protected IMediator _mediator;
    protected System.Timers.Timer? _timer;

    /// <summary>
    /// When Execution Timeout reaches, the OperationLock will be released automatically.
    /// </summary>
    /// <returns></returns>
    protected TimeSpan _executionTimeout = new(0, 10, 0); // 10 min
    private readonly bool _needLock = true;
    private readonly bool _needLog = true;

    public ScheduledBackgroundService(ILogger logger, IServiceProvider services, bool needLock, bool needLog)
    {
        _logger = logger;
        _services = services;
        //_lockManager = _services.GetRequiredService<IOperationLockManager>();
        _needLock = needLock;
        _needLog = needLog;
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Starting " + GetType().ToString());
        CreateCronSchedule();
        await ScheduleNextRun(cancellationToken);
    }

    private void CreateCronSchedule()
    {
        var cronExpression = GetCronExpression();
        _cronSchedule = CrontabSchedule.Parse(cronExpression,
            new CrontabSchedule.ParseOptions { IncludingSeconds = false });
    }

    protected async Task<bool> IsLockedAsync()
    {
        var lockKey = GetType().Name;
        //return _lockManager.IsLockedAsync(lockKey, _executionTimeout);
        return await Task.FromResult(false);
    }

    protected async Task<bool> ReleaseLockAsync()
    {
        var lockKey = GetType().Name;
        return await Task.FromResult(true);
        //return _lockManager.ReleaseLockAsync(lockKey);
    }

    protected async Task RunTask(CancellationToken cancellationToken)
    {
        if (!_needLock || !await IsLockedAsync())
        {
            Guid jobLogId = Guid.NewGuid();
            if (_needLog)
            {
                var createNewJobLogCommand = new CreateNewScheduledJobLogCommand(GetType().Name);
                var jobLog = await _mediator.Send(createNewJobLogCommand, cancellationToken);
                jobLogId = jobLog.Id;
            }
            using var scope = _services.CreateScope();
            try
            {
                _mediator = scope.ServiceProvider.GetService<IMediator>();
                await ExecuteAsync(scope, cancellationToken);
                if (_needLog)
                {
                    await SetJobAsSucceeded(jobLogId);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error On Running " + GetType().ToString());
                if (_needLog)
                {
                    await SetAsFailed(jobLogId, e.Message);
                }
            }
            finally
            {
                if (_needLock)
                {
                    await ReleaseLockAsync();
                }
            }
        }
        else
        {
            _logger.Warning("Couldn't Get Lock for Scheduled Background Service.{@name}", GetType().Name);
        }
    }

    private async Task SetAsFailed(Guid jobLogId, string message)
    {
        try
        {
            var finishJobCommand = new CreateFinishJobLogCommand(jobLogId, Domain.Aggregates.ScheduledJobLogAggregate.ScheduledJobLogStatus.Failed, message);
            await _mediator.Send(finishJobCommand);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error on logging scheduled job {GetType().Name}. Error: {ex.Message}");

        }

    }

    private async Task SetJobAsSucceeded(Guid jobLogId)
    {
        var finishJobCommand = new CreateFinishJobLogCommand(jobLogId, Domain.Aggregates.ScheduledJobLogAggregate.ScheduledJobLogStatus.Succeeded);
        await _mediator.Send(finishJobCommand);
    }

    protected async Task ScheduleNextRun(CancellationToken cancellationToken)
    {
        var nextRunTime = _cronSchedule.GetNextOccurrence(DateTime.UtcNow);
        var delay = nextRunTime - DateTimeOffset.UtcNow;
        if (delay.TotalMilliseconds <= 0) // prevent non-positive values from being passed into Timer
        {
            await ScheduleNextRun(cancellationToken);
        }

        _timer = new System.Timers.Timer(delay.TotalMilliseconds);
        _timer.Elapsed += async (sender, args) =>
        {
            _timer.Dispose(); // reset and dispose timer
            _timer = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await RunTask(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error On RunTask " + GetType().ToString());
                }
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                await ScheduleNextRun(cancellationToken); // reschedule next
            }
        };
        _timer.Start();
        await Task.CompletedTask;
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        _logger.Information("Stopped " + GetType().ToString());
        await Task.CompletedTask;
    }

    protected abstract string GetCronExpression();

    protected abstract Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken);

    public void Dispose()
    {
        _timer?.Dispose();
    }
}


