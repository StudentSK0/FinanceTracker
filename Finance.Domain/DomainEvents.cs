using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Domain;

public interface IDomainEvent { }

public interface IEventHandler<TEvent> where TEvent : IDomainEvent
{
    void Handle(TEvent evt);
}

public static class DomainEvents
{
    private static readonly List<object> _handlers = new();

    public static void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IDomainEvent
        => _handlers.Add(handler);

    public static void Raise<TEvent>(TEvent evt) where TEvent : IDomainEvent
    {
        foreach (var handler in _handlers.OfType<IEventHandler<TEvent>>())
            handler.Handle(evt);
    }
}
