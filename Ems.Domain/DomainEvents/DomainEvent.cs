using Ems.Domain.Primitives;

namespace Ems.Domain.DomainEvents;

public abstract record DomainEvent(Guid Id) : IDomainEvent;
