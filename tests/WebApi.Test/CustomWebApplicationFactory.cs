using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using CommonTestUtilities.Entities;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using WebApi.Test.Resources;
using CashFlow.Domain.Enums;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense_Team_Member { get; private set; } = default!;
    public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<CashFlowDbContext>(c =>
                {
                    c.UseInMemoryDatabase("InMemoryDbForTesting");
                    c.UseInternalServiceProvider(provider);
                });
                var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var encripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
                StartDatabase(context, encripter, tokenGenerator);

            });
    }

    private void StartDatabase(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(context, passwordEncripter, tokenGenerator);
        Expense_Team_Member = AddExpenses(context, userTeamMember, expenseId: 1, tagId: 1);

        var userAdmin = AddUserAdmin(context, passwordEncripter, tokenGenerator);
        Expense_Admin = AddExpenses(context, userAdmin, expenseId: 2, tagId: 2);

        context.SaveChanges();
    }

    private User AddUserTeamMember(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;
        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);
        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);
        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;
        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);
        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);
        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private ExpenseIdentityManager AddExpenses(CashFlowDbContext context, User user, long expenseId, long tagId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        foreach (var tag in expense.Tags)
        {
            tag.Id = tagId;
            tag.ExpenseId = expenseId;
        }

        context.Expenses.Add(expense);

        return new ExpenseIdentityManager(expense);
    }
}