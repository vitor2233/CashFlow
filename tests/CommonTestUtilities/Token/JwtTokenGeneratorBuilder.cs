
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token;

public static class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(acGenerator => acGenerator.Generate(It.IsAny<User>())).Returns("token");

        return mock.Object;
    }
}