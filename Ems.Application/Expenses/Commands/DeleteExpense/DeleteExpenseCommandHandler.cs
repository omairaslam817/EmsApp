using Ems.Domain.Repositories;
using Ems.Infrastructure.ViewModels.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Commands.DeleteExpense
{
    internal class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, CqrsResponseViewModel>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CqrsResponseViewModel> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _expenseRepository.GetSingleOrDefaultAsync(f => f.Id == request.ExpenseId);

            if (expense == null)
            {
                return new CqrsResponseViewModel { ErrorMessage = "Expense not found." };
            }

            try
            {
                _unitOfWork.BeginTransaction();

                _expenseRepository.Remove(expense);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new CqrsResponseViewModel { ErrorMessage = $"Error deleting expense: {ex.Message}" };
            }

            return new CqrsResponseViewModel();
        }
    }
}
