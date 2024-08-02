
using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.LoggedUser;
public static class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(l => l.Get()).ReturnsAsync(user);
        return mock.Object;
    }
}