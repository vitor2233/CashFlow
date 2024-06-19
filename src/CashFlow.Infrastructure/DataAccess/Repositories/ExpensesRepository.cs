using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context;
    public ExpensesRepository(CashFlowDbContext context)
    {
        _context = context;
    }
    public async Task Add(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        if (result is null) return false;

        _context.Expenses.Remove(result);
        return true;
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _context.Expenses.AsNoTracking().OrderBy(e => e.Title).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
    }

    public void Update(Expense expense)
    {
        _context.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(
            year: date.Year,
            month: date.Month,
            day: 1,
            hour: 0,
            minute: 0,
            second: 0,
            DateTimeKind.Utc).Date;
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(
            year: date.Year,
            month: date.Month,
            day: daysInMonth,
            hour: 23,
            minute: 59,
            second: 59,
            DateTimeKind.Utc);

        return await _context.Expenses.AsNoTracking()
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Title)
            .ToListAsync();
    }
}