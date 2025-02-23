using RAG.AI.Domain.DomainEvents.ScheduledJobLogDomainEvents;
using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;

public class ScheduledJobLog : AggregateRoot<Guid>
{

    /// <summary>
    /// Use init fields if you need to init most of fields in constructor
    /// </summary>
    public string JobName { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public string? ErrorMessage { get; private set; }
    public ScheduledJobLogStatus Status { get; private set; }
    public bool IsCompleted => Status != ScheduledJobLogStatus.InProgress;

    public ScheduledJobLog(string jobName) : base()
    {

        JobName = jobName;
        StartTime = DateTime.Now;
        Status = ScheduledJobLogStatus.InProgress;
    }

    public void SetAsSucceeded()
    {
        Status = ScheduledJobLogStatus.Succeeded;
        EndTime = DateTime.Now;
    }
    public void SetAsFailed(string errorMessage)
    {
        Status = ScheduledJobLogStatus.Failed;
        EndTime = DateTime.Now;
        ErrorMessage = errorMessage;
        RaiseDomainEvent(new ScheduledJobFailedEvent(this));
    }
}

