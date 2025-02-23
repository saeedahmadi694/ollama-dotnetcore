namespace RAG.AI.Domain.Exceptions.TicketFiles;

public class TicketFileDomainNotFoundException : Exception
{
    public TicketFileDomainNotFoundException()
    { }

    public TicketFileDomainNotFoundException(long id) : base($"TicketFile with id {id} not found")
    { }
}




