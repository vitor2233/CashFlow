using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpensesTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;
    public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {

        var result = await DoGet(METHOD, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("expenses").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}