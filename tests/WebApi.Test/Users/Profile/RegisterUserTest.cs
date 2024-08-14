using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace WebApi.Test.Users.Profile;

public class GetUserProfileTest : CashFlowClassFixture
{
    private const string METHOD = "api/User";
    private readonly string _token;
    private readonly string _userName;
    private readonly string _email;

    public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _userName = webApplicationFactory.User_Team_Member.GetName();
        _email = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(_userName);
        response.RootElement.GetProperty("email").GetString().Should().Be(_email);
    }
}