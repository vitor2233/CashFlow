
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public interface IDeleteExpenseUseCase
{
    Task Execute(long id);
}
