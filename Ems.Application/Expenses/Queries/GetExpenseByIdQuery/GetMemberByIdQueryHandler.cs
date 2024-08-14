using Ems.Application.Abstractions.Messaging;
using Ems.Domain.Repositories;
using Ems.Domain.Shared;

namespace Ems.Application.Members.Queries.GetExpenseByIdQuery;

internal sealed class GetMemberByIdQueryHandler
    : IQueryHandler<GetExpenseByIdQuery, ExpenseResponse>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetMemberByIdQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Result<ExpenseResponse>> Handle(
        GetExpenseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var member = await _expenseRepository.GetSingleOrDefaultAsync(
           m=> m.Id == request.ExpenseId);

        if (member is null)
        {
            return Result.Failure<ExpenseResponse>(new Error(
                "Expense.NotFound",
                $"The expense with Id {request.ExpenseId} was not found"));
        }

        var response = new ExpenseResponse(member.Id, member.TransactionDetails,member.Balance,member.TransactionDate,member.TransactionAmount,member.TransactionNumber);

        return response;
    }
}