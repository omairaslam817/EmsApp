using Ems.Application.Abstractions;
using Ems.Domain.Entities;

namespace Ems.Infrastructure.Services;

internal sealed class EmailService : IEmailService
{
    public Task SendWelcomeEmailAsync(Member member, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task SendInvitationSentEmailAsync(Member member, Gathering gathering, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task SendInvitationAcceptedEmailAsync(Gathering gathering, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
