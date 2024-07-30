using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;

    public PasswordEncripterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();
        _mock.Setup(pEncripter => pEncripter.Encrypt(It.IsAny<string>())).Returns("!123Encripted!");
    }

    public PasswordEncripterBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
            _mock.Setup(r => r.Verify(password, It.IsAny<string>())).Returns(true);
        return this;
    }

    public IPasswordEncripter Build() => _mock.Object;
}