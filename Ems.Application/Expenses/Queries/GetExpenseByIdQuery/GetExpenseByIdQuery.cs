using Ems.Application.Abstractions.Messaging;

namespace Ems.Application.Members.Queries.GetExpenseByIdQuery;

public sealed record GetExpenseByIdQuery(Guid ExpenseId) : IQuery<ExpenseResponse>;