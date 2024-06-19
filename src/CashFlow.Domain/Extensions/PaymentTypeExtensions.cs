namespace CashFlow.Domain.Extensions;
public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this int paymentType)
    {
        return paymentType switch
        {
            0 => "Dinheiro",
            1 => "Cartão de Crédito",
            2 => "Cartão de Débito",
            3 => "Transferência Bancaria",
            _ => ""
        };
    }
}