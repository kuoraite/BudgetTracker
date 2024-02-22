using System.ComponentModel.DataAnnotations;
using BudgetTracker.Enums;

namespace BudgetTracker.ViewModels
{
    public class DateViewModel
    {
        public Months Month { get; set; }

        [Required]
        [RegularExpression(@"^20[0-9][0-9]$", ErrorMessage = "Invalid year format")]
        public int Year { get; set; }
    }
}
