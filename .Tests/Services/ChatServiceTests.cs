// ChatServiceTests.cs

using Yggdrasil.Tests.Util;
using Yggdrasil.Services;
using Yggdrasil.DTO;
using Yggdrasil.Models;


namespace Yggdrasil.Tests.Services;

public class ChatServiceTests : DatabaseTestBase{
    private readonly ChatService _service;
    private readonly Faker _faker = new();
    private readonly FakeHttpHandler _handler;

    public ChatServiceTests(DatabaseFixture fixture) : base (fixture){
        _handler = new FakeHttpHandler();
        var client = new HttpClient(_handler);
        _service = new ChatService(client);
    }

    [Fact]
    public async Task ChatService_Send(){
        _handler.Response = JsonSerializer.Serialize(new
        {
            id = _faker.Random.Guid(),
            @object = "Chat_completion",
            created = _faker.Random.Int(min: 1000),
            model = _faker.Lorem.Lines(),
            provider = _faker.Lorem.Lines(),
            native_finish_reason = "stop",
            message = new {role= "assistant", content="Hello, what's up?"}
            
        }, new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        var connection = new LLMConnection {Name= "someConnection",APIType = APIType.chatCompletion, APIKey = _faker.Random.Guid().ToString(), URL = _faker.Internet.Url(), Reasoning = false };
        var payload = new LLMPayload();

        var result = await _service.Send(connection, payload);

        Assert.NotNull(result);
    }
}

         
