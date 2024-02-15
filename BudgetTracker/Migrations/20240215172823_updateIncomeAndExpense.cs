using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetTracker.Migrations
{
    /// <inheritdoc />
    public partial class updateIncomeAndExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Incomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Incomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Expenses");
        }
    }
}
