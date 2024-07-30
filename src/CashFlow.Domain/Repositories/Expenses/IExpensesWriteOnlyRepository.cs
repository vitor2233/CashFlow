using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
    void Update(Expense expense);

    /// <summary>
    /// This function returns true if deletion was successful, otherwise returns false
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(long id);
}