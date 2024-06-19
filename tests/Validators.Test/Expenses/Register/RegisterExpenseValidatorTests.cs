using CashFlow.Application.UseCases.Expenses;
using CommonTestUtilities;
using FluentAssertions;

namespace Validators.Test;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = title;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Title is required."));
    }

    [Fact]
    public void Error_Date_Future()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Expenses cannot be for the future."));
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.PaymentType = 900;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Payment Type is not valid."));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Error_Amount_Invalid(decimal amount)
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Amount = amount;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals("Amount must be greater than zero."));
    }
}