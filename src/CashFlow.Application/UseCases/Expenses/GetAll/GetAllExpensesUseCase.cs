using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllExpensesUseCase(IExpensesReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpensesJson>>(result)
        };
    }
}
