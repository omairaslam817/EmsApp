using Ems.Application.Abstractions.Messaging;

namespace Ems.Application.Members.GetMemberById;

public sealed record GetMemberByIdQuery(Guid MemberId) : IQuery<MemberResponse>;