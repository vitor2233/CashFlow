using Bogus;
using CashFlow.Domain.Entities;

namespace CommonTestUtilities.Entities;
public static class ExpenseBuilder
{
    public static List<Expense> Collection(User user, uint count = 2)
    {
        var list = new List<Expense>();

        if (count == 0)
            count = 1;

        var expenseId = 1;

        for (int i = 0; i < count; i++)
        {
            var expense = Build(user);
            expense.Id = expenseId++;

            list.Add(expense);
        }

        return list;
    }
    public static Expense Build(User user)
    {
        return new Faker<Expense>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Title, faker => faker.Commerce.ProductName())
            .RuleFor(u => u.Description, faker => faker.Commerce.ProductName())
            .RuleFor(u => u.Date, faker => faker.Date.Past())
            .RuleFor(u => u.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(u => u.PaymentType, faker => faker.Random.Int(min: 0, max: 3))
            .RuleFor(u => u.UserId, _ => user.Id)
            .RuleFor(u => u.Tags, faker => faker.Make(1, () => new Tag
            {
                Id = 1,
                Value = faker.PickRandom<CashFlow.Domain.Enums.Tag>(),
                ExpenseId = 1
            }));
    }
}
