using BudgetTracker.Enums;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models
{
    public class Expense : ITransaction
    {
        [Required]
        public int ExpenseId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }

        public int Year { get; set; }
        public Months Month { get; set; }

        public int BudgetId { get; set; } //Foreign key to Budget
        public Budget Budget { get; set; }//Navigation property

        public int? CategoryId { get; set; } //Foreign key to Category
        public Category Category { get; set; } // Navigation property
    }
}
