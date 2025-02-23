namespace RAG.AI.Application.Commands.SendEmail;

public class SendEmailCommand : IRequest<bool>
{
    public string ReceiverEmail { get; init; }
    public string Subject { get; init; }
    public string Message { get; init; }
}

