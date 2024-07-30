using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities;
public static class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(r => r.Email, faker => faker.Internet.Email())
            .RuleFor(u => u.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
