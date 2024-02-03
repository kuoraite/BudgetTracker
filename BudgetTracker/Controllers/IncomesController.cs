using BudgetTracker.Models;
using BudgetTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    public class IncomesController : Controller
    {
        public readonly BudgetTrackerDbContext context;

        public IncomesController(BudgetTrackerDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult CreateIncome(BudgetDetailsViewModel viewModel)
        {
            Income newIncome = viewModel.NewIncome;

            context.Incomes.Add(newIncome);
            context.SaveChanges();

            return RedirectToAction("details", "budgets", new { id = viewModel.NewIncome.BudgetId });
        }
    }
}
