// ChatService.cs
using System.Net.Http.Headers;
using System.Xml.Linq;
using Yggdrasil.Data;
using Yggdrasil.DTO;
using Yggdrasil.Util;
using Yggdrasil.Models;
using System.Text;
using System.Text.Json;

namespace Yggdrasil.Services;

public class ChatService
{

    private readonly HttpClient _client;
    public ChatService(HttpClient client){
        _client = client;
    }
    
    ///<summary></summary>
    ///<param name="connection">The LLM Connection object, which contains APIKey, URL, Model, etc.</param>
    ///<param name="payload">The messages/chatlogs to send to the LLM</param>
    ///<returns>The LLM response</returns>
    public async Task<ServiceResult<Object>> Send(LLMConnection connection, LLMPayload payload)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", connection.APIKey);

        var body = ObjectMerger.Merge([payload, connection]);
        body["reasoning"] = JsonSerializer.SerializeToElement(new { enabled = connection.Reasoning });
        body.Remove("name");
        var json = JsonSerializer.Serialize(body, new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync(connection.URL, content);
        var result = await response.Content.ReadAsStringAsync();

        return new(result);
    }
}
