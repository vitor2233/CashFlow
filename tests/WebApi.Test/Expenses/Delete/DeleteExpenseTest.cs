using System.Net;
using System.Text.Json;
using CommonTestUtilities;
using FluentAssertions;

namespace WebApi.Test.Expenses.Delete;

public class DeleteExpenseIdTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses/";
    private readonly string _token;
    private readonly long _expenseId;
    public DeleteExpenseIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_Team_Member.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete($"{METHOD}{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var resultGet = await DoGet($"{METHOD}{_expenseId}", token: _token);

        resultGet.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var result = await DoDelete($"{METHOD}{100}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals("Expense not found."));
    }
}