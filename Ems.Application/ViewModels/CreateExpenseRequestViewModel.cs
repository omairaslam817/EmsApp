using System.ComponentModel.DataAnnotations;

namespace Ems.Application.ViewModels.Factories;

public class CreateExpenseRequestViewModel
{

    [Required]
    public DateTime TransactionDate { get; set; }

    [Required]
    public string TransactionNumber { get; set; }

    [Required]
    public string TransactionDetails { get; set; }

    public decimal TransactionAmount { get; set; }

    public decimal Balance { get; set; }
}
public class GetExpenseRequestViewModel: CreateExpenseRequestViewModel
{
    public Guid Id { get; set; }
}