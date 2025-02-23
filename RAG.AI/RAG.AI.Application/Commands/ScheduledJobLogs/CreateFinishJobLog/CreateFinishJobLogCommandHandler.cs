using RAG.AI.Application.Exceptions.ScheduledJobLogExceptions;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Dtos.ScheduledJobLog;

namespace RAG.AI.Application.Commands.ScheduledJobLogs.CreateFinishJobLog;

public class CreateFinishJobLogCommandHandler : IRequestHandler<CreateFinishJobLogCommand, ScheduledJobLogDto>
{
    public IUnitOfWork _uow;

    public CreateFinishJobLogCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ScheduledJobLogDto> Handle(CreateFinishJobLogCommand request, CancellationToken cancellationToken)
    {
        var scheduledJobLog = await _uow.ScheduledJobLogRepository.GetAsync(request.Id) ?? throw new ScheduledJobLogNotFoundException(request.Id);

        if (request.Status.IsSucceeded)
        {
            scheduledJobLog.SetAsSucceeded();
        }
        else
        {
            scheduledJobLog.SetAsFailed(request.ErrorMessage);
        }

        _uow.ScheduledJobLogRepository.Update(scheduledJobLog);
        await _uow.SaveChangesAsync(cancellationToken);
        return new ScheduledJobLogDto(scheduledJobLog);
    }
}

