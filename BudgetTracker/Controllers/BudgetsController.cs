using BudgetTracker.Enums;
using BudgetTracker.Models;
using BudgetTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    public class BudgetsController : Controller
    {
        public readonly BudgetTrackerDbContext context;

        public BudgetsController(BudgetTrackerDbContext context)
        {
            this.context = context;
        }

        // GET: BudgetsController
        public ActionResult Index()
        {
            var budgets = context.Budgets.ToList();
            return View(budgets);
        }

        // GET: BudgetsController/Details/5
        public ActionResult Details(int id)
        {
            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == id);

            if (budget == null) return NotFound();

            BudgetDetailsViewModel viewModel = new()
            {
                Budget = budget,
                Expenses = context.Expenses.Where(x => x.BudgetId == id).ToList(),
                Incomes = context.Incomes.Where(x => x.BudgetId == id).ToList(),
                Categories = context.Categories.ToList()
            };

            return View(viewModel);
        }

        public ActionResult GetIncomesAndExpensesByDate(BudgetDetailsViewModel model, int budgetId)
        {
           /* if (!ModelState.IsValid)
            {
                return View("Details", model);
            }*/

            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == budgetId);

            if (budget == null) return NotFound();

            BudgetDetailsViewModel viewModel = new()
            {
                Budget = budget,
                Expenses = context.Expenses.Where(x => x.BudgetId == budget.BudgetId && x.Year == model.DateViewModel.Year && x.Month == model.DateViewModel.Month).ToList(),
                Incomes = context.Incomes.Where(x => x.BudgetId == budget.BudgetId && x.Year == model.DateViewModel.Year && x.Month == model.DateViewModel.Month).ToList(),
                Categories = context.Categories.ToList(),
                DateViewModel = model.DateViewModel
            };

            return View("Details", viewModel);
        }

        // GET: BudgetsController/Create
        public ActionResult Create()
        {
            ViewBag.Action = "create";

            return View();
        }

        // POST: BudgetsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Budget budget)
        {
            if (ModelState.IsValid)
            {
                if(budget.TotalAmount == null)
                {
                    budget.TotalAmount = 0;
                }
                context.Budgets.Add(budget);
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }

        // GET: BudgetsController/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Action = "edit";

            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == id);
            if (budget == null) return NotFound();

            return View(budget);
        }

        // POST: BudgetsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Budget budget)
        {
            if (ModelState.IsValid)
            {
                if (budget.TotalAmount == null)
                {
                    budget.TotalAmount = 0;
                }
                context.Budgets.Update(budget);
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }

        // GET: BudgetsController/Delete/5
        public ActionResult Delete(int id)
        {
            var budget = context.Budgets.Find(id);

            if (budget == null) return NotFound();

            context.Budgets.Remove(budget);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult EditDescription(string budgetItemType, int transactionId, string description)
        {
            if (budgetItemType == "Income")
            {
                var incomeId = transactionId;
                return RedirectToAction("EditDescription", "Incomes", new { incomeId, description });
            }
            else if (budgetItemType == "Expense")
            {
                var expenseId = transactionId;
                return RedirectToAction("EditDescription", "Expenses", new { expenseId, description });
            }

            return NotFound();
        }
    }
}
