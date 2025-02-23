using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;

public class ScheduledJobLogStatus : Enumeration
{
    public static ScheduledJobLogStatus InProgress = new(1, nameof(InProgress).ToLowerInvariant());
    public static ScheduledJobLogStatus Succeeded = new(2, nameof(Succeeded).ToLowerInvariant());
    public static ScheduledJobLogStatus Failed = new(3, nameof(Failed).ToLowerInvariant());
    protected ScheduledJobLogStatus() { }
    protected ScheduledJobLogStatus(int id, string name) : base(id, name)
    {
    }
    public static IEnumerable<ScheduledJobLogStatus> List()
    {
        return new[] { InProgress, Succeeded, Failed };
    }

    public bool IsFailed => Id == Failed.Id;
    public bool IsSucceeded => Id == Succeeded.Id;

    public static ScheduledJobLogStatus FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        return state == null
            ? throw new Exception($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}")
            : state;
    }

    public static ScheduledJobLogStatus From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        return state == null
            ? throw new Exception($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}")
            : state;
    }
}

