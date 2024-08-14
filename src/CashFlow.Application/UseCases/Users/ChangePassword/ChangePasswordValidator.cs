using CashFlow.Application.UseCases.Users.Validator;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(u => u.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
