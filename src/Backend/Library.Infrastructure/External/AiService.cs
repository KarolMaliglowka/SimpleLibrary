using System.Text;
using System.Text.Json;
using Library.Application.Services;
using Microsoft.Extensions.Configuration;

namespace Library.Infrastructure.External;

public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _model = "gpt-5.5";

    public AiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                configuration["OpenAI:ApiKey"]);
    }

    public async Task<string> AskAsync(string question)
    {
        var requestBody = new
        {
            model = _model,
            input = question
        };

        var response = await _httpClient.PostAsync(
            "responses",
            new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}