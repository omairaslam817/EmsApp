using Ems.Domain.Repositories;
using Ems.Infrastructure.ViewModels.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Commands.UpdateExpense
{
    internal class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, CqrsResponseViewModel>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExpenseCommandHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CqrsResponseViewModel> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _expenseRepository.GetSingleOrDefaultAsync(f => f.Id == request.Model.Id);

            if (expense == null)
            {
                return new CqrsResponseViewModel { ErrorMessage = "Expense not found." };
            }

            expense.TransactionDate = request.Model.TransactionDate;
            expense.TransactionDetails = request.Model.TransactionDetails;
            expense.Balance = request.Model.Balance;
            expense.TransactionAmount = request.Model.TransactionAmount;
            expense.TransactionNumber = request.Model.TransactionNumber;

            try
            {
                _unitOfWork.BeginTransaction();

                _expenseRepository.Update(expense);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new CqrsResponseViewModel { ErrorMessage = $"Error updating expense: {ex.Message}" };
            }

            return new CqrsResponseViewModel();
        }
    }
}
