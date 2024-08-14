using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public static class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(u => u.Password, (faker, u) => faker.Internet.Password())
            .RuleFor(u => u.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
