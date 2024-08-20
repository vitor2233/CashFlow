using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Users.Delete;

public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute();
        await act.Should().NotThrowAsync();
    }

    private DeleteUserAccountUseCase CreateUseCase(User user)
    {
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new DeleteUserAccountUseCase(writeRepository, unitOfWork, loggedUser);
    }
}