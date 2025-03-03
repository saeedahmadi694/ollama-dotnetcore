using RAG.AI.Domain.DomainEvents.Imports;
using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.Aggregates.ImportAggregate;
public class ImportJob : AggregateRoot<int>
{
    public int UserId { get; private set; }
    public Guid UniqueId { get; private set; }
    public string FileAddress { get; private set; }
    public string FileName { get; private set; }
    public ImportJobStatus Status { get; private set; }

    private ImportJob()
    {
        UniqueId = Guid.NewGuid();
    }
    public ImportJob(int userId, string fileAddress, string fileName) : this()
    {
        UserId = userId;
        FileAddress = fileAddress;
        FileName = fileName;
        Status = ImportJobStatus.Created;
    }

    public ImportJob SetAsInProgress()
    {
        if (Status.IsInProgress)
            return this;
        Status = ImportJobStatus.InProgress;
        //RaiseDomainEvent(new ImportJobStartedDomainEvent(Id));
        return this;
    }
    public ImportJob SetAsFailed()
    {
        if (Status.IsFailed)
            return this;
        Status = ImportJobStatus.Failed;
        //RaiseDomainEvent(new ImportJobStatusChangedDomainEvent(Id));
        return this;
    }
    public ImportJob SetAsSucceeded()
    {
        if (Status.IsSucceeded)
            return this;
        Status = ImportJobStatus.Succeeded;
        //RaiseDomainEvent(new ImportJobStatusChangedDomainEvent(Id));
        return this;
    }

    public ImportJob SetAsCreated()
    {
        Status = ImportJobStatus.Created;
        //if (DomainEvents.Any(e => e.GetType().Name == typeof(NewImportJobCreatedDomainEvent).Name))
        //    return this;
        //RaiseDomainEvent(new NewImportJobCreatedDomainEvent(Id));
        return this;
    }

    //public ImportJob SetItemAsDuplicate(ImportJobItem item)
    //{
    //    _items.First(e => e == item)
    //        .SetAsDuplicate();
    //    if (AllItemsFinished)
    //        SetAsSucceeded();
    //    return this;
    //}

    //public ImportJob SetItemAsFinished(ImportJobItem item)
    //{
    //    _items.First(e => e.Id == item.Id)
    //        .SetAsFinished();
    //    if (AllItemsFinished)
    //        SetAsSucceeded();
    //    return this;
    //}

    //public ImportJob SetItemAsUnProcessable(ImportJobItem item)
    //{
    //    _items.First(e => e == item)
    //     .SetAsUnProcessable();
    //    if (AllItemsFinished)
    //        SetAsSucceeded();
    //    return this;
    //}

}
