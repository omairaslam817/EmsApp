using Ems.Infrastructure.ViewModels.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Commands.DeleteExpense
{
    public class DeleteExpenseCommand : IRequest<CqrsResponseViewModel>
    {
        public Guid ExpenseId { get; set; }
    }
}
