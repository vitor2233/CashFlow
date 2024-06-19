
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseJson> Execute(long id);
}
