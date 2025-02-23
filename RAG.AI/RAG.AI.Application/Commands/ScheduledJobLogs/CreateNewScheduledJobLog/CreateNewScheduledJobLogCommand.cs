using RAG.AI.Infrastructure.Dtos.ScheduledJobLog;

namespace RAG.AI.Application.Commands.ScheduledJobLogs.CreateNewScheduledJobLog;

public record CreateNewScheduledJobLogCommand(string JobName) : IRequest<ScheduledJobLogDto> { }

