using System.Net;
using System.Text.Json;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace WebApi.Test.Users.Update;

public class ChangePasswordTest : CashFlowClassFixture
{
    private const string METHOD = "api/User/change-password";
    private readonly string _token;
    private readonly string _email;
    private readonly string _password;

    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _email = webApplicationFactory.User_Team_Member.GetEmail();
        _password = webApplicationFactory.User_Team_Member.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var response = await DoPatch(METHOD, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };
        response = await DoPost("api/login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost("api/login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

    }

    [Fact]
    public async Task Error_Password_Different_Current_Password()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = await DoPatch(METHOD, request, _token);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals("Invalid password."));
    }
}