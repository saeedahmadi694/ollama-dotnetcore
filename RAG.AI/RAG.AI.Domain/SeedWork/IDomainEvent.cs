﻿namespace RAG.AI.Domain.SeedWork;

public interface IDomainEvent
{
}

public interface INotificationDomainEvent : IDomainEvent, INotification
{

}

public interface IMessageDomainEvent : IDomainEvent
{

}



