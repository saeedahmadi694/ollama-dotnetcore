namespace RAG.AI.Domain.Exceptions.TicketDiscussions;

public class TicketDiscussionDomainNotFoundException : Exception
{
    public TicketDiscussionDomainNotFoundException()
    { }

    public TicketDiscussionDomainNotFoundException(long id) : base($"TicketDiscussion with id {id} not found")
    { }
}




