using System.Text.RegularExpressions;
using CashFlow.Communication.Requests;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.UseCases.Users.Validator;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ERROR_MESSAGE = "Senha deve conter no mínimo 8 caracteres, pelo menos uma letra maiúscula, uma letra minúscula, número e um caractere especial";
    private const string ERROR_MESSAGE_KEY = "ErrorMessage";
    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE_KEY}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ERROR_MESSAGE);
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ERROR_MESSAGE);
            return false;
        }

        if (!UpperCaseRegex().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ERROR_MESSAGE);
            return false;
        }

        if (!LowerCaseRegex().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ERROR_MESSAGE);
            return false;
        }

        if (!NumbersRegex().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ERROR_MESSAGE);
            return false;
        }

        if (!SpecialRegex().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ERROR_MESSAGE);
            return false;
        }

        return true;
    }

    [GeneratedRegex("[A-Z]+")]
    private static partial Regex UpperCaseRegex();
    [GeneratedRegex("[a-z]+")]
    private static partial Regex LowerCaseRegex();
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumbersRegex();
    [GeneratedRegex("[\\!\\?\\*\\.]+")]
    private static partial Regex SpecialRegex();
}
