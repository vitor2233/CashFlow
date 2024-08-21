using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Domain.Entities;
[Table("T_TAGS")]
public class Tag
{
    public long Id { get; set; }
    public Enums.Tag Value { get; set; }
    public long ExpenseId { get; set; }
    public Expense Expense { get; set; } = default!;
}
