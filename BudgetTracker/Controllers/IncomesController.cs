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

        public IActionResult DeleteIncome(int id)
        {
            var income = context.Incomes.FirstOrDefault(x => x.IncomeId == id);

            if (income == null) return NotFound();

            var budgetId = income.BudgetId;

            context.Remove(income);
            context.SaveChanges();

            return RedirectToAction("details", "budgets", new { id = budgetId});
        }

        public IActionResult EditDescription(string description, int incomeId)
        {
            var income = context.Incomes.FirstOrDefault(x => x.IncomeId == incomeId);
            if (income == null) return NotFound();

            income.Description = description;
            context.SaveChanges();

            var budgetId = income.BudgetId;

            return RedirectToAction("Details", "Budgets", new { id =  budgetId});
        }
    }
}
