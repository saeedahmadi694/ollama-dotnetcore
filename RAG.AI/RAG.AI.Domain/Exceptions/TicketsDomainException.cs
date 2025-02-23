namespace RAG.AI.Domain.Exceptions;

public class TicketsDomainException : Exception
{
    public TicketsDomainException()
    { }

    public TicketsDomainException(string message)
        : base(message)
    { }

    public TicketsDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}




