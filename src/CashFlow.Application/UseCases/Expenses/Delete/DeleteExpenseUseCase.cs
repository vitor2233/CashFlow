using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseUseCase(IExpensesWriteOnlyRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var result = await _repository.Delete(id);

        if (!result)
        {
            throw new NotFoundException("Despesa não encontrada");
        }

        await _unitOfWork.Commit();

    }
}
