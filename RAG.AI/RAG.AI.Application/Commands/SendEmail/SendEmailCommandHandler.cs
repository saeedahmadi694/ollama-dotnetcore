using BookHouse.Communication.Infrastructure.ExternalServices.Email;

namespace RAG.AI.Application.Commands.SendEmail;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, bool>
{
    private readonly IEmailSender _emailSender;

    public SendEmailCommandHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        return await _emailSender.SendEmail(request.ReceiverEmail, request.Subject, request.Message);
    }
}

