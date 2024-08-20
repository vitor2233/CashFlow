using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Users.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteUserAccountUseCase(IUserWriteOnlyRepository writeOnlyRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.Get();

        await _writeOnlyRepository.Delete(loggedUser);

        await _unitOfWork.Commit();
    }
}
