using BudgetTracker.Models;
using BudgetTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    public class ExpensesController : Controller
    {
        public readonly BudgetTrackerDbContext context;
        public ExpensesController(BudgetTrackerDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult CreateExpense(BudgetDetailsViewModel viewModel)
        {
            Expense newExpense = viewModel.NewExpense;

            context.Expenses.Add(newExpense);
            context.SaveChanges();

            return RedirectToAction("details", "budgets", new { id = viewModel.NewExpense.BudgetId });
        }
    }
}
