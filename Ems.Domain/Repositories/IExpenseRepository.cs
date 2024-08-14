using Ems.Core.Helpers;
using Ems.Domain.Entities;
using Ems.Domain.Interfaces;

namespace Ems.Domain.Repositories;

public interface IExpenseRepository: IRepository<Expense>
{
    Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<Expense>> GetExpensesAsync(string sortColumn, string sortOrder, string searchTerm, int page,
        int pageSize);
}