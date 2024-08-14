using MediatR;

namespace Ems.Domain.Primitives;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
