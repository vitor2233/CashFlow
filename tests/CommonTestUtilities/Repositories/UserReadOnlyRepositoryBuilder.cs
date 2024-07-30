
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(r => r.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public void GetUserByEmail(User user)
    {
        _repository.Setup(r => r.GetUserByEmail(user.Email)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}