using Ems.Application.ViewModels.Factories;
using Ems.Core.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Queries.GetAllExpensesQuery
{
    public class GetAllExpensesQuery : IRequest<PagedList<GetExpenseRequestViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}
