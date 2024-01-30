using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetTracker.Models
{
    public class Income
    {
        [Required]
        public int IncomeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }

        public int BudgetId { get; set; } //Foreign key to Budget
        public Budget Budget {get;set; }//Navigation property

        public int? CategoryId { get; set; } //Foreign key to Category
        public Category Category { get; set; } // Navigation property
    }
}
