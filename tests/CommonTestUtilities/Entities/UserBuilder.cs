using Bogus;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;
public static class UserBuilder
{
    public static User Build()
    {
        var passwordEncripter = new PasswordEncripterBuilder().Build();
        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Email))
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid());
        return user;
    }
}
