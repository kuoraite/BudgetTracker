using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Models
{
    public class BudgetTrackerDbContext : DbContext
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }

        public BudgetTrackerDbContext(DbContextOptions options) : base(options) { }
    }
}
