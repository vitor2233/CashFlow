using CashFlow.Application.UseCases.Expenses;
using CashFlow.Application.UseCases.Users.Validator;
using CashFlow.Communication.Requests;
using CommonTestUtilities;
using FluentAssertions;
using FluentValidation;

namespace Validators.Test;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("teste")]
    [InlineData("Testeeee")]
    [InlineData("TESTEEEE")]
    [InlineData("Testeee1")]
    [InlineData("Testeeee1")]
    public void Error_Password_Invalid(string password)
    {
        //Arrange
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        //Act
        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        //Assert
        result.Should().BeFalse();
    }
}