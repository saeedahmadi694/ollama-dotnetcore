using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.Aggregates.ImportAggregate;
public class ImportJobStatus : Enumeration
{
    public static ImportJobStatus Created = new(1, nameof(Created).ToLowerInvariant());
    public static ImportJobStatus InProgress = new(2, nameof(InProgress).ToLowerInvariant());
    public static ImportJobStatus Failed = new(3, nameof(Failed).ToLowerInvariant());
    public static ImportJobStatus Succeeded = new(4, nameof(Succeeded).ToLowerInvariant());
    protected ImportJobStatus() { }
    protected ImportJobStatus(int id, string name) : base(id, name)
    {
    }
    public static IEnumerable<ImportJobStatus> List()
    {
        return new[] { InProgress, Succeeded, Failed, Created };
    }

    public bool IsFailed => Id == Failed.Id;
    public bool IsSucceeded => Id == Succeeded.Id;
    public bool IsCreated => Id == Created.Id;
    public bool IsInProgress => Id == InProgress.Id;

    public static ImportJobStatus FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        return state == null
            ? throw new Exception($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}")
            : state;
    }

    public static ImportJobStatus From(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);

        return state == null
            ? throw new Exception($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}")
            : state;
    }
}


