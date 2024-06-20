using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();
        var act = async ()=> await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(e=> e.GetErrors().Count == 1 && e.GetErrors().Contains("Name is required."));
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);
        var act = async ()=> await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(e=> e.GetErrors().Count == 1 && e.GetErrors().Contains("Email já está sendo utilizado"));
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var jwtTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readOnlyUserRepository = new UserReadOnlyRepositoryBuilder();
        if(!string.IsNullOrWhiteSpace(email))
        {
            readOnlyUserRepository.ExistActiveUserWithEmail(email);
        }
        return new RegisterUserUseCase(mapper, passwordEncripter, readOnlyUserRepository.Build(), writeOnlyRepository, unitOfWork, jwtTokenGenerator);
    }
}
