using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;
public class ExpenseIdentityManager(Expense expense)
{
    private readonly Expense _expense = expense;

    public long GetExpenseId() => _expense.Id;
    public DateTime GetExpenseDate() => _expense.Date;
}