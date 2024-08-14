using Ems.Application.Abstractions.Messaging;
using Ems.Application.ViewModels.Factories;
using Ems.Infrastructure.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Commands.CreateExpense
{
    public  class CreateExpenseCommand
    
        : ICommand<ExpenseResponseModel>
    {
        public IFormFile File { get; set; }

    }
}
