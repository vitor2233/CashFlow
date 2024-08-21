using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Response;

public class ResponseExpenseJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int PaymentType { get; set; }
    public IList<Tag> Tags { get; set; } = [];
}