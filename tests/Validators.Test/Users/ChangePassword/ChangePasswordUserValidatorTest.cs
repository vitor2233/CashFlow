using CashFlow.Application.UseCases.Expenses;
using CashFlow.Application.UseCases.Users.ChangePassword;
using CommonTestUtilities;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Users.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Password_Empty()
    {
        //Arrange
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Senha deve conter no mínimo 8 caracteres, pelo menos uma letra maiúscula, uma letra minúscula, número e um caractere especial"));
    }
}