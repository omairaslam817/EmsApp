using Ems.Core.Helpers;
using Ems.Domain.Entities;
using Ems.Domain.Repositories;
using Ems.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Persistence.Repository
{
    internal class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
        }

        private ApplicationDbContext _appContext => _context;

        public async Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return null;
        }
        public async Task<PagedList<Expense>> GetExpensesAsync(string sortColumn, string sortOrder, string searchTerm,
        int page, int pageSize)
        {
            IQueryable<Expense> expensesQuery = _appContext.Expenses.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(searchTerm))
                expensesQuery = expensesQuery.Where(p => p.TransactionNumber.Contains(searchTerm));

            if (sortOrder?.ToLower() == "desc")
                expensesQuery = expensesQuery.OrderByDescending(GetSortProperty(sortColumn));
            else
                expensesQuery = expensesQuery.OrderBy(GetSortProperty(sortColumn));

            var factories = expensesQuery
                .AsSingleQuery();


            var factoriesListResponsesQuery = factories;
            var factoriesResult = await PagedList<Expense>.CreateAsync(factoriesListResponsesQuery, page, pageSize);

            return factoriesResult;
        }
        private static Expression<Func<Expense, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "id" => expense => expense.Id,
                "transactiondate" => expense => expense.TransactionDate,
                "transactionnumber" => expense => expense.TransactionNumber,
                "transactiondetails" => expense => expense.TransactionDetails,
                "transactionamount" => expense => expense.TransactionAmount,
                "balance" => expense => expense.Balance,
                _ => expense => expense.Id
            };
        }

    }
}
