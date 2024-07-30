using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using FluentAssertions;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/Login";
    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _password;
    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
        _password = webApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson { Email = _email, Password = _password };

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }
}