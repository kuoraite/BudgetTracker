using System.ComponentModel.DataAnnotations;
using BudgetTracker.Enums;

namespace BudgetTracker.ViewModels
{
    public class DateViewModel
    {
        [Required]
        public Months Month { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
