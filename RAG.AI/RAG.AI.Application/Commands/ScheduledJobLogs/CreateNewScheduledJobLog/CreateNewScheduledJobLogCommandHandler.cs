using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Dtos.ScheduledJobLog;

namespace RAG.AI.Application.Commands.ScheduledJobLogs.CreateNewScheduledJobLog;

public class CreateNewScheduledJobLogCommandHandler : IRequestHandler<CreateNewScheduledJobLogCommand, ScheduledJobLogDto>
{
    private readonly IUnitOfWork _uow;

    public CreateNewScheduledJobLogCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ScheduledJobLogDto> Handle(CreateNewScheduledJobLogCommand request, CancellationToken cancellationToken)
    {
        var scheduledJobLog = new ScheduledJobLog(request.JobName);
        var insertionResult = await _uow.ScheduledJobLogRepository.InsertAsync(scheduledJobLog);
        await _uow.SaveChangesAsync(cancellationToken);

        return new ScheduledJobLogDto(insertionResult);

    }
}

