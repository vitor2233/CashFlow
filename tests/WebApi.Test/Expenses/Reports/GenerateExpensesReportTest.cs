using System.Net;
using System.Net.Mime;
using System.Text.Json;
using CommonTestUtilities;
using FluentAssertions;

namespace WebApi.Test.Expenses.Reports;

public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string METHOD = "api/Report";
    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;
    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
        _expenseDate = webApplicationFactory.Expense_Admin.GetExpenseDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet($"{METHOD}/pdf?month={_expenseDate:MM-yyyy}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet($"{METHOD}/excel?month={_expenseDate:MM-yyyy}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet($"{METHOD}/excel?month={_expenseDate:MM-yyyy}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet($"{METHOD}/pdf?month={_expenseDate:MM-yyyy}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}