using RAG.AI.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.Domain.DomainEvents.Imports;
public record ImportJobStartedDomainEvent(int JobId) : IMessageDomainEvent
{
}
