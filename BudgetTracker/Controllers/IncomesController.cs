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
            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == newIncome.BudgetId);

            if (budget == null) return NotFound();

            budget.TotalAmount += newIncome.Amount;

            context.Incomes.Add(newIncome);
            context.Budgets.Update(budget);
            context.SaveChanges();

            return RedirectToAction("GetIncomesAndExpensesWithNewData", "Budgets", (new { newIncome.BudgetId, newIncome.Year, newIncome.Month }));
        }

        public IActionResult DeleteIncome(int id)
        {
            var income = context.Incomes.FirstOrDefault(x => x.IncomeId == id);

            if (income == null) return NotFound();

            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == income.BudgetId);
            if(budget == null) return NotFound();

            budget.TotalAmount -= income.Amount;

            context.Remove(income);
            context.Budgets.Update(budget);
            context.SaveChanges();

            return RedirectToAction("details", "budgets", new { id = budget.BudgetId});
        }

        [HttpGet]
        public IActionResult EditDescription(int incomeId, string description)
        {
            return ProcessEditDescription(incomeId, description);
        }

        [HttpPost]
        public IActionResult ProcessEditDescription(int incomeId, string description)
        {
            var result = CommonEditDescriptionLogic(incomeId, description);

            if (result is ViewResult)
            {
                return result;
            }

            var budgetId = ((ObjectResult)result).Value as int?;
            if (budgetId.HasValue)
            {
                return RedirectToAction("Details", "Budgets", new { id = budgetId.Value });
            }

            return NotFound();
        }

        private IActionResult CommonEditDescriptionLogic(int incomeId, string description)
        {
            var income = context.Incomes.FirstOrDefault(x => x.IncomeId == incomeId);
            if (income == null)
            {
                return NotFound();
            }

            income.Description = description;
            context.SaveChanges();

            return Ok(income.BudgetId);
        }
    }
}
