using Bogus;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Yggdrasil.Models;
using Yggdrasil.Services;
using Yggdrasil.DTO;

namespace Tests.Endpoints;

public class ChatServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly ChatService _service;
    private readonly Bogus.Faker _faker;

    public ChatServiceTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _service = new ChatService(_db);
        _faker = new Bogus.Faker();
    }

    /*
        Helper to generate chat requests, for use with generate chat logs
    */
    private List<ChatMessage> generateMessages(int count, Guid? ID=null)
    {
        return Enumerable.Range(0, count).Select(i => new ChatMessage{Content = _faker.Lorem.Sentence(), Conversation_ID = ID}).ToList();
    }
    /*
        Helper to generate chat logs
    */
    private List<ChatLogs> generateChatLogs(List<ChatMessage> requests)
    {
        return requests.Select(c => _service.CreateMessage(c)).ToList();
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void CreateMessage_GeneratesNewGuid()
    {
        var request = new ChatMessage {Content = "Hello"};
        var result = _service.CreateMessage(request);
        Assert.IsType<Guid>(result.Conversation_ID);
        Assert.NotEqual(Guid.Empty, result.Conversation_ID);
    }

    [Fact]
    public void CreateMessage_NewConversationDefaultTitle()
    {
        var request = new ChatMessage { Content = "hello" };

        var result = _service.CreateMessage(request);

        Assert.NotNull(result.Title);
        Assert.Equal(result.Title, $"Conversation: {result.Conversation_ID.ToString().Substring(0,30)}");
    }

    [Fact]
    public void CreateMessage_NoNewTitleOnFollowingMessages()
    {
        var request = new ChatMessage { Content = "first" };
        var first = _service.CreateMessage(request);

        var second = _service.CreateMessage(new ChatMessage
        {
            Conversation_ID = first.Conversation_ID,
            Content = "second"
        });

        Assert.Null(second.Title);
    }

    [Fact]
    public void CreateMessage_SavesToDB()
    {
        var request = new ChatMessage { Content = "hello" };

        _service.CreateMessage(request);

        Assert.Equal(1, _db.ChatLogs.Count());
    }


    [Fact]
    public void GetConversations_GetAllConversations()
    {
        int conversationCount = _faker.Random.Int(3, 10);
        var results = generateChatLogs(generateMessages(conversationCount));
        var fetched = _service.GetConversations();
        Assert.Equal(fetched.Count, conversationCount);
    }

    [Fact]
    public void GetMessages_getCorrectMessages()
    {
        int conversationsCount = _faker.Random.Int(3,10);
        var results = generateChatLogs(generateMessages(conversationsCount));
        ChatLogs toGet = results[_faker.Random.Int(0, conversationsCount - 1)];

        List<ChatLogs> result = _service.GetMessages(toGet.Conversation_ID, null);
        Assert.Equal(toGet, result[0]);
    }

    [Fact]
    public void getMessages_GetsAllMessages()
    {
        int count = _faker.Random.Int(5, 15);
        Guid ID = Guid.NewGuid();
        List<ChatMessage> requests = generateMessages(count, ID);
        List<ChatLogs> results = generateChatLogs(requests);
        List<ChatLogs> fetched = _service.GetMessages(ID, null);

        Assert.Equal(count, fetched.Count);
        
        for (int i = 0; i < count; i++)
        {
            Assert.Equal(requests[i].Content, fetched[i].Content);
        }
        Assert.True(fetched.Zip(fetched.Skip(1)).All(pair => pair.First.TimeStamp <= pair.Second.TimeStamp));
    }

    [Fact]
    public void getMessages_GetOnlyRequestedCount()
    {
        int count = 10;
        int toFetch = 5;
        Guid ID = Guid.NewGuid();
        generateChatLogs(generateMessages(count, ID));
        List<ChatLogs> fetched = _service.GetMessages(ID, toFetch);

        Assert.Equal(fetched.Count, toFetch);
    }
    
    [Fact]
    public void GetMessages_NonExistentId_ReturnsEmpty()
    {
        var result = _service.GetMessages(Guid.NewGuid(), null);
        Assert.Empty(result);
    }
}