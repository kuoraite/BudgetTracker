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

            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == newExpense.BudgetId);
            if (budget == null) return NotFound();

            budget.TotalAmount -= newExpense.Amount;

            context.Expenses.Add(newExpense);
            context.Budgets.Update(budget);
            context.SaveChanges();

            return RedirectToAction("GetIncomesAndExpensesWithNewData", "Budgets", (new { newExpense.BudgetId, newExpense.Year, newExpense.Month }));
        }

        public IActionResult DeleteExpense(int id)
        {
            var expense = context.Expenses.FirstOrDefault(x => x.ExpenseId == id);
            if (expense == null) return NotFound();

            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == expense.BudgetId);
            if (budget == null) return NotFound();

            budget.TotalAmount += expense.Amount;

            context.Remove(expense);
            context.Budgets.Update(budget);
            context.SaveChanges();

            return RedirectToAction("details", "budgets", new { id = budget.BudgetId });
        }

        [HttpGet]
        public IActionResult EditDescription(int expenseId, string description)
        {
            return ProcessEditDescription(expenseId, description);
        }

        [HttpPost]
        public IActionResult ProcessEditDescription(int expenseId, string description)
        {
            var result = CommonEditDescriptionLogic(expenseId, description);

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

        private IActionResult CommonEditDescriptionLogic(int expenseId, string description)
        {
            var expense = context.Expenses.FirstOrDefault(x => x.ExpenseId == expenseId);
            if (expense == null)
            {
                return NotFound();
            }

            expense.Description = description;
            context.SaveChanges();

            return Ok(expense.BudgetId);
        }
    }
}
