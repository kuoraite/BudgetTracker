using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models
{
    public class Budget
    {
        [Required]
        public int BudgetId { get; set; }
        public string? Name { get; set; } 
        public decimal TotalAmount { get; set; } 
    }
}
