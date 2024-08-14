using Ems.Application.Abstractions.Messaging;

namespace Ems.Application.Members.UpdateMember;

public sealed record UpdateMemberCommand(Guid MemberId, string FirstName, string LastName) : ICommand;
