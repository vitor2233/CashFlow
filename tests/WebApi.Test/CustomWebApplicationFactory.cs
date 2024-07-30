using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using CommonTestUtilities.Entities;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user;
    private string _password;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
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
                StartDatabase(context, encripter);
            });
    }

    private void StartDatabase(CashFlowDbContext context, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = passwordEncripter.Encrypt(_user.Password);
        context.Users.Add(_user);
        context.SaveChanges();
    }
}