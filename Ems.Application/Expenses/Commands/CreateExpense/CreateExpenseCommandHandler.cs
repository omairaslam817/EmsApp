using Ems.Application.Abstractions.Messaging;
using Ems.Application.ViewModels.Factories;
using Ems.Domain.Entities;
using Ems.Domain.Helpers;
using Ems.Domain.Interfaces;
using Ems.Domain.Repositories;
using Ems.Domain.Shared;
using Ems.Domain.ValueObjects;
using Ems.Infrastructure.ViewModels.Response;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ems.Application.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, ExpenseResponseModel>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CreateExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IUnitOfWork unitOfWork, IFileService fileService)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result<ExpenseResponseModel>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var filePath = await _fileService.SaveFile(request.File, cancellationToken);
            var expenseRequests = ExcelHelper.Import<CreateExpenseRequestViewModel>(filePath);

            var expenses = new List<Expense>();

            foreach (var item in expenseRequests)
            {
                var transactionNumberResult = TransactionNumber.Create(item.TransactionNumber);
                if (transactionNumberResult.IsFailure)
                {
                    return Result.Failure<ExpenseResponseModel>(transactionNumberResult.Error);
                }

                var expenseModel = new Expense
                {
                    CreatedDate = item.TransactionDate,
                    CreatedBy = "",
                    UpdatedBy = "",
                    TransactionAmount = item.TransactionAmount,
                    TransactionDate = item.TransactionDate,
                    TransactionNumber = transactionNumberResult.Value.ToString(),
                    Balance = item.Balance,
                    TransactionDetails = item.TransactionDetails,
                };

                expenses.Add(expenseModel);
            }

            try
            {
                _unitOfWork.BeginTransaction();

                _expenseRepository.AddRange(expenses);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return Result.Failure<ExpenseResponseModel>(new Error("Error creating expenses", ex.Message));
            }

            var result = new ExpenseResponseModel { Id = 1, Name = "Expenses created successfully" };
            return Result.Success(result);
        }
    }

    public class ExpenseResponseModel : CqrsResponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
