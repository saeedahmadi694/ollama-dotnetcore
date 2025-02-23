using RAG.AI.Application.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAG.AI.Infrastructure.Exceptions.BaseExceptions;

namespace RAG.AI.Application.Exceptions.ScheduledJobLogExceptions;

public class ScheduledJobLogNotFoundException : NotFoundException
{
    public ScheduledJobLogNotFoundException(string message) : base(message) { }

    public ScheduledJobLogNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public ScheduledJobLogNotFoundException(Guid id) : base($"ScheduledJobLog with id {id} can not be found") { }
}



