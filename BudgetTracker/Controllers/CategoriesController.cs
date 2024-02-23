using BudgetTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    public class CategoriesController : Controller
    {
        public readonly BudgetTrackerDbContext context;

        public CategoriesController(BudgetTrackerDbContext context)
        {
            this.context = context;
        }

        // GET: CategoriesController
        public ActionResult Index()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }

        // GET: CategoriesController/Create
        public ActionResult Create()
        {
            ViewBag.Action = "create";
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: CategoriesController/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Action = "edit";

            var category = context.Categories.FirstOrDefault(x => x.CategoryId == id);

            if(category == null) return NotFound();

            return View(category);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Update(category);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            var category = context.Categories.Find(id);

            if(category == null ) return NotFound();

            UpdateRelatedTransactions(context.Incomes, id);
            UpdateRelatedTransactions(context.Expenses, id);
            context.SaveChanges();

            context.Categories.Remove(category);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private static void UpdateRelatedTransactions(IQueryable<ITransaction> transactions, int categoryId)
        {
            var transactionsToUpdate = transactions.Where(x => x.CategoryId == categoryId);
            if (transactionsToUpdate != null)
            {
                foreach (var transaction in transactionsToUpdate)
                {
                    transaction.CategoryId = null;
                }
            }
        }
    }
}
