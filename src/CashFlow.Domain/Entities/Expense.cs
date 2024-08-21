using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Domain.Entities;

[Table("T_EXPENSES")]
public class Expense
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int PaymentType { get; set; }
    public ICollection<Tag> Tags { get; set; } = [];
    public long UserId { get; set; }
    public User User { get; set; } = default!;
}
