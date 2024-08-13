using System.Net;
using System.Text.Json;
using CommonTestUtilities;
using FluentAssertions;

namespace WebApi.Test.Expenses.Update;

public class UpdateExpenseIdTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses/";
    private readonly string _token;
    private readonly long _expenseId;
    public UpdateExpenseIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_Team_Member.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut($"{METHOD}{_expenseId}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPut($"{METHOD}{_expenseId}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals("Title is required."));
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut($"{METHOD}{100}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals("Expense not found."));
    }
}