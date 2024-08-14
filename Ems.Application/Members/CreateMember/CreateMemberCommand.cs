using Ems.Application.Abstractions.Messaging;

namespace Ems.Application.Members.CreateMember;

public sealed record CreateMemberCommand(
    string Email,
    string FirstName,
    string LastName) : ICommand<Guid>;
