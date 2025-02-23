using RAG.AI.Application.Behaviors;
using RAG.AI.Infrastructure.Dtos.Users;

namespace RAG.AI.Application.Commands.LoginWithImpersonationToken;

public record LoginWithImpersonationTokenCommand(string ImpersonationToken) : ICommand<SignInResponseDto>
{
}
public class LoginWithImpersonationTokenCommandValidator : AbstractValidator<LoginWithImpersonationTokenCommand>
{
    public LoginWithImpersonationTokenCommandValidator()
    {
        RuleFor(x => x.ImpersonationToken)
            .NotEmpty().WithMessage("توکن جانشینی نباید خالی باشد.");
    }
}

