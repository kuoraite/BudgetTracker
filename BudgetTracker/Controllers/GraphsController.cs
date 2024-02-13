using BudgetTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers
{
    public class GraphsController : Controller
    {
        public readonly BudgetTrackerDbContext context;

        public GraphsController(BudgetTrackerDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetIncomesData(string itemType)
        {
            var query = itemType == "Incomes" ? context.Incomes.Cast<ITransaction>() :
            itemType == "Expenses" ? context.Expenses.Cast<ITransaction>() :
            null;

            if (query == null) return NoContent();

            var categorySum = query
                .GroupBy(x => x.CategoryId)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    Sum = group.Sum(x => x.Amount)
                })
                .ToList();

            var categoryNames = context.Categories
                .Where(x => categorySum.Select(xs => xs.CategoryId).Contains(x.CategoryId))
                .Select(x => x.Name)
                .ToList();

            var missingCategories = categorySum.Where(cs => cs.CategoryId == null)
                                    .Select(cs => "None");
            categoryNames.AddRange(missingCategories);

            var data = new
            {
                CategorySum = categorySum,
                CategoryNames = categoryNames,
                Colors = GenerateRandomColors(categoryNames.Count)
            };

            return Json(data);
        }

        private static List<string> GenerateRandomColors(int count)
        {
            var random = new Random();
            var colors = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                colors.Add(color);
            }

            return colors;
        }
    }
}