﻿using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.DomainEvents.Imports;
public record ImportJobStatusChangedDomainEvent(int Id) : IMessageDomainEvent
{
}
