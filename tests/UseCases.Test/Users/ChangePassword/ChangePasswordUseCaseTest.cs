using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_New_Password_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(e => e.GetErrors().Count == 1 && e.GetErrors().Contains("Senha deve conter no mínimo 8 caracteres, pelo menos uma letra maiúscula, uma letra minúscula, número e um caractere especial"));
    }

    [Fact]
    public async Task Error_Current_Password_Different()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(e => e.GetErrors().Count == 1 && e.GetErrors().Contains("Invalid password."));
    }

    private ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var updateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();

        return new ChangePasswordUseCase(loggedUser, passwordEncripter, updateOnlyRepository, unitOfWork);
    }
}
