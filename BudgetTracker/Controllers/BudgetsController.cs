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
            int year = DateTime.Today.Year;
            Months month = (Months)DateTime.Today.Month;

            var viewModel = GetBudgetDetailsViewModel(id, year, month);
            if(viewModel == null) return NotFound();            

            return View(viewModel);
        }

        public ActionResult GetIncomesAndExpensesByDate(BudgetDetailsViewModel model, int budgetId)
        {
            var viewModel = GetBudgetDetailsViewModel(budgetId, model.DateViewModel.Year, model.DateViewModel.Month);
            if (viewModel == null) return NotFound();

            return View("Details", viewModel);
        }

        private BudgetDetailsViewModel GetBudgetDetailsViewModel(int budgetId, int year, Months month)
        {
            var budget = GetBudgetById(budgetId);
            if (budget == null) return null;

            return new BudgetDetailsViewModel
            {
                Budget = budget,
                Expenses = context.Expenses
                    .Where(x => x.BudgetId == budgetId && x.Year == year && x.Month == month)
                    .ToList(),
                Incomes = context.Incomes
                    .Where(x => x.BudgetId == budgetId && x.Year == year && x.Month == month)
                    .ToList(),
                Categories = context.Categories.ToList(),
                DateViewModel = new()
                {
                    Year = year,
                    Month = month
                }
            };
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

            var budget = GetBudgetById((int)id); 
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
            var budget = GetBudgetById(id);
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

        private Budget GetBudgetById(int id)
        {
            return context.Budgets.FirstOrDefault(x => x.BudgetId == id);
        }
    }
}
