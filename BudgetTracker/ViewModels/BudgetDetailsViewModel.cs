﻿using BudgetTracker.Models;

namespace BudgetTracker.ViewModels
{
    public class BudgetDetailsViewModel
    {
        public Budget Budget { get; internal set; }
        public List<Expense> Expenses { get; set; }
        public List<Income> Incomes { get; set; }

        public Income NewIncome { get; set; }
    }
}
