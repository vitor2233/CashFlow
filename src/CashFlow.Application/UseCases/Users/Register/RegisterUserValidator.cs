using CashFlow.Application.UseCases.Users.Validator;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .When(u => !string.IsNullOrWhiteSpace(u.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage("Email is invalid.");
        RuleFor(u => u.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}
