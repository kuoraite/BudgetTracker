using BudgetTracker.Enums;

namespace BudgetTracker.Models
{
    public interface ITransaction
    {
        int? CategoryId { get; set; }
        
        int BudgetId { get; set; }
        int Year { get; set; }
        Months Month { get; set; }

        decimal Amount { get; set; }
    }
}
