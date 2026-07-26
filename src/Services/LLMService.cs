// LLMService.cs
using System.Text.Json;

namespace Yggdrasil.Services;

public class LLMService
{
    private readonly HttpClient _client;
    public LLMService(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> Chat(List<object> messages)
    {
        var response = await _client.PostAsJsonAsync("/v1/chat/completions", new
        {
            messages
        });

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("choices")[0]
                     .GetProperty("messages")
                     .GetProperty("content")
                     .GetString() ?? "";
    }

    public string GenerateBasePrompt(Guid World_ID)
    {
        return "";
    }
}