
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseLoginJson> Execute(RequestLoginJson request);
}
