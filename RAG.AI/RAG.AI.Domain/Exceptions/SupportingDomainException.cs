namespace RAG.AI.Domain.Exceptions;

public class SupportingDomainException : Exception
{
    public SupportingDomainException()
    { }

    public SupportingDomainException(string message)
        : base(message)
    { }

    public SupportingDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}




