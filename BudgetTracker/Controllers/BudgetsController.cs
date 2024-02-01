using BudgetTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

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
            return View();
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
                context.Budgets.Update(budget);
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }

        // GET: BudgetsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BudgetsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
