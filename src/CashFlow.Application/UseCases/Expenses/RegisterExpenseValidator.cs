using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;

public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public ExpenseValidator()
    {
        RuleFor(e => e.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(e => e.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(e => e.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expenses cannot be for the future.");
        RuleFor(e => e.PaymentType).ExclusiveBetween(-1, 4).WithMessage("Payment Type is not valid.");
        RuleFor(e => e.Tags).ForEach(rule =>
        {
            rule.IsInEnum().WithMessage("Tag is not valid.");
        });
    }
}
