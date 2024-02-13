namespace BudgetTracker.Models
{
    public interface ITransaction
    {
        int? CategoryId { get; set; }
        decimal Amount { get; set; }
    }
}
