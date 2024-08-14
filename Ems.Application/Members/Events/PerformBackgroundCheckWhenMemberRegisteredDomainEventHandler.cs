using Ems.Application.Abstractions.Messaging;
using Ems.Domain.DomainEvents;

namespace Ems.Application.Members.Events;

internal sealed class PerformBackgroundCheckWhenMemberRegisteredDomainEventHandler
    : IDomainEventHandler<MemberRegisteredDomainEvent>
{
    public Task Handle(
        MemberRegisteredDomainEvent notification,
        CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
