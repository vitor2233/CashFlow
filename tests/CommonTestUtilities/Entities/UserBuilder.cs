using Bogus;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;
public static class UserBuilder
{
    public static User Build(string role = Roles.TEAM_MEMBER)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Build();
        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Email))
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
            .RuleFor(u => u.Role, _ => role)
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid());
        return user;
    }
}
