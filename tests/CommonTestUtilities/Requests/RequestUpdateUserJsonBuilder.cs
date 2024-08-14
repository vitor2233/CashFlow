using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, u) => faker.Internet.Email(u.Name));
    }
}
