namespace RAG.AI.Domain.Exceptions.TicketCategories;

public class TicketCategoryDomainNotFoundException : Exception
{
    public TicketCategoryDomainNotFoundException()
    { }

    public TicketCategoryDomainNotFoundException(long id) : base($"TicketCategory with id {id} not found")
    { }
}




