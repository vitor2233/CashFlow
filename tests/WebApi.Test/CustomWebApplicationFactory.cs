using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using CommonTestUtilities.Entities;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private Expense _expense;
    private User _user;
    private string _password;
    private string _token;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetToken() => _token;
    public long GetExpenseId() => _expense.Id;
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
                StartDatabase(context, encripter);

                _token = tokenGenerator.Generate(_user);
            });
    }

    private void StartDatabase(CashFlowDbContext context, IPasswordEncripter passwordEncripter)
    {
        AddUser(context, passwordEncripter);
        AddExpenses(context, _user);
        context.SaveChanges();
    }

    private void AddUser(CashFlowDbContext context, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = passwordEncripter.Encrypt(_user.Password);
        context.Users.Add(_user);
    }

    private void AddExpenses(CashFlowDbContext context, User user)
    {
        _expense = ExpenseBuilder.Build(user);
        context.Expenses.Add(_expense);
        context.SaveChanges();
    }
}