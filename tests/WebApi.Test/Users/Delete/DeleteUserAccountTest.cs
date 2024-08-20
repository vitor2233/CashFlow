using System.Net;
using FluentAssertions;

namespace WebApi.Test.Users.Delete;

public class DeleteUserAccountTest : CashFlowClassFixture
{
    private const string METHOD = "api/User/";
    private readonly string _token;
    public DeleteUserAccountTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete($"{METHOD}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}