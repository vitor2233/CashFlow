using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "")
    {
        AuthorizeRequest(token);
        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string requestUri, string token = "")
    {
        AuthorizeRequest(token);
        return await _httpClient.GetAsync(requestUri);
    }

    private void AuthorizeRequest(string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}