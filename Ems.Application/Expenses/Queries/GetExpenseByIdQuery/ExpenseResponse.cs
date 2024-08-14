namespace Ems.Application.Members.Queries.GetExpenseByIdQuery;

public sealed record ExpenseResponse(Guid Id, string transactionDetails,decimal balance,DateTime transactionDate, decimal transactionAmount,string transactionNumber);