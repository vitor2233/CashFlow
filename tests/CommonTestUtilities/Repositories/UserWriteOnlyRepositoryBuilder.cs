
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;
public static class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();
        return mock.Object;
    }
}