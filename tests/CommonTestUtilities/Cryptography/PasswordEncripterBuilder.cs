using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public static class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        var mock = new Mock<IPasswordEncripter>();

        mock.Setup(pEncripter => pEncripter.Encrypt(It.IsAny<string>())).Returns("!Encripted!");

        return mock.Object;
    }
}