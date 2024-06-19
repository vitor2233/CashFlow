using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Login.Validator;

public class DoLoginValidator : AbstractValidator<RequestLoginJson>
{
    public DoLoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email e/ou senha inválidos");
    }
}
