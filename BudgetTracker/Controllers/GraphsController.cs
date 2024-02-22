using BudgetTracker.Enums;
using BudgetTracker.Models;
using BudgetTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BudgetTracker.Controllers
{
    public class GraphsController : Controller
    {
        public readonly BudgetTrackerDbContext context;

        public GraphsController(BudgetTrackerDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int budgetId, int year, Months month)
        {
            var budget = context.Budgets.FirstOrDefault(x => x.BudgetId == budgetId);
            if (budget == null) return NotFound();

            BudgetDetailsViewModel detailsViewModel = new()
            {
                Budget = budget,
                DateViewModel = new()
                {
                    Year = year,
                    Month = month
                }
            };

            return View(detailsViewModel);
        }

        public IActionResult GetDoughnutChartData(string itemType, int budgetId, int year, string month)
        {
            var query = itemType == "Incomes" ? context.Incomes.Cast<ITransaction>() :
            itemType == "Expenses" ? context.Expenses.Cast<ITransaction>() :
            null;

            if (query == null) return NoContent();

            var categorySum = query
                .Where(x => x.BudgetId == budgetId && x.Year == year && x.Month == (Months)Enum.Parse(typeof(Months), month, true))
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

        public IActionResult GetBarChartData(int budgetId, int year, string month)
        {
            var (incomeAmounts, incomeLabels) = GetAmounts(context.Incomes, budgetId, year, (Months)Enum.Parse(typeof(Months), month, true));
            var (expenseAmounts, expenseLabels) = GetAmounts(context.Expenses, budgetId, year, (Months)Enum.Parse(typeof(Months), month, true));


            if (!incomeLabels.SequenceEqual(expenseLabels))
            {
                return BadRequest();
            }
            
            var labels = incomeLabels;

            var data = new
            {
                IncomeAmounts = incomeAmounts,
                ExpenseAmounts = expenseAmounts,
                Labels = labels,
                Colors = GenerateRandomColors(2)
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

        private (List<decimal>, List<string>) GetAmounts(IQueryable<ITransaction> transactions, int budgetId, int year, Months targetMonth)
        {
            var amounts = new List<decimal>();
            var labels = new List<string>();

            for (int i = 0; i < 6; i++)
            {
                var amountsForCurrentMonth = transactions
                    .Where(x => x.BudgetId == budgetId && x.Year == year && x.Month == targetMonth)
                    .Select(x => x.Amount)
                    .Sum();

                amounts.Add(amountsForCurrentMonth);
                labels.Add(targetMonth.ToString());

                if (targetMonth == Months.January)
                {
                    targetMonth = Months.December;
                    year--;
                }
                else
                {
                    targetMonth = (Months)(int)targetMonth - 1;
                }
            }

            return (amounts, labels);
        }
    }
}