using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase(IExpensesReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var result = await _repository.GetById(id);

        if (result is null)
        {
            throw new NotFoundException("Despesa não encontrada");
        }

        return _mapper.Map<ResponseExpenseJson>(result);

    }
}
