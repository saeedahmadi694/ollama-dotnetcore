using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;
using RAG.AI.Infrastructure.Dtos.ScheduledJobLog;

namespace RAG.AI.Application.Commands.ScheduledJobLogs.CreateFinishJobLog;

public record CreateFinishJobLogCommand(Guid Id, ScheduledJobLogStatus Status, string? ErrorMessage = null) : IRequest<ScheduledJobLogDto>
{
}

public class CreateFinishJobLogCommandValidator : AbstractValidator<CreateFinishJobLogCommand>
{
    public CreateFinishJobLogCommandValidator()
    {
        RuleFor(e => e.Status).NotEqual(ScheduledJobLogStatus.InProgress);
        RuleFor(e => e.ErrorMessage)
            .MinimumLength(1)
            .When(e => e.Status == ScheduledJobLogStatus.Failed);
    }
}

