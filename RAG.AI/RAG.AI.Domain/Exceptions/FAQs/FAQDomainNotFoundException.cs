namespace RAG.AI.Domain.Exceptions.FAQs;

public class FAQDomainNotFoundException : Exception
{
    public FAQDomainNotFoundException()
    { }

    public FAQDomainNotFoundException(long id) : base($"FAQ with id {id} not found")
    { }
}




