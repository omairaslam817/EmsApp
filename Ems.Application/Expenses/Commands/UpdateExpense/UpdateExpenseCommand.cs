using Ems.Application.ViewModels;
using Ems.Infrastructure.ViewModels.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseCommand : IRequest<CqrsResponseViewModel>
    {
        public UpdateExpenseRequestViewModel Model { get; set; }
    }
}
