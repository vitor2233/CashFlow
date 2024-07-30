using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Response;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _readOnlyRepository;
    private readonly IExpensesWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpenseUseCase(IExpensesWriteOnlyRepository writeOnlyRepository, IExpensesReadOnlyRepository readOnlyRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var expense = _readOnlyRepository.GetById(loggedUser, id);

        if (expense is null)
        {
            throw new NotFoundException("Despesa não encontrada");
        }

        await _writeOnlyRepository.Delete(id);

        await _unitOfWork.Commit();
    }
}
