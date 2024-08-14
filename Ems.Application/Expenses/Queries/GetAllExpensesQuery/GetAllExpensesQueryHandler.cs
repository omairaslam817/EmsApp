using AutoMapper;
using Ems.Application.ViewModels.Factories;
using Ems.Core.Helpers;
using Ems.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Queries.GetAllExpensesQuery
{
    public class GetAllExpensesQueryHandler: IRequestHandler<GetAllExpensesQuery, PagedList<GetExpenseRequestViewModel>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public GetAllExpensesQueryHandler(IExpenseRepository expenseRepository,
             IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }
        public async Task<PagedList<GetExpenseRequestViewModel>> Handle(GetAllExpensesQuery request,
       CancellationToken cancellationToken)
        {
            var factoriesList = await _expenseRepository.GetExpensesAsync(request.SortColumn, request.SortOrder,
                request.SearchTerm, request.PageNumber, request.PageSize);

            var factoryViewModels = _mapper.Map<List<GetExpenseRequestViewModel>>(factoriesList.Items);

            var pagedList = new PagedList<GetExpenseRequestViewModel>(
                factoryViewModels,
                request.PageNumber,
                request.PageSize,
                factoriesList.TotalCount);

            return pagedList;
        }
    }
}
