using CashFlow.Application.UseCases.Login;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);
        request.Email = user.Email;

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();
        result.Where(e => e.GetErrors().Count == 1 && e.GetErrors().Contains("Email e/ou senha inválidos"));
    }

    [Fact]
    public async Task Password_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();
        result.Where(e => e.GetErrors().Count == 1 && e.GetErrors().Contains("Email e/ou senha inválidos"));
    }

    private DoLoginUseCase CreateUseCase(User user, string? password = null)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
        var jwtTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readOnlyUserRepository = new UserReadOnlyRepositoryBuilder();
        readOnlyUserRepository.GetUserByEmail(user);
        return new DoLoginUseCase(readOnlyUserRepository.Build(), passwordEncripter, jwtTokenGenerator);
    }
}
