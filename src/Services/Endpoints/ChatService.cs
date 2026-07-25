using System.Data.Common;
using Yggdrasil.Models;
using Yggdrasil.DTO;

namespace Yggdrasil.Services;

public class ChatService{
    private readonly AppDbContext _db;
    public ChatService(AppDbContext db)
    {
        _db = db;
    }
    /// <summary>
    /// Creates a user message
    /// </summary>
    /// <param name="request">ChatMessage request format</param>
    /// <returns>The Chatlog object of the message</returns>
    public ChatLogs CreateMessage (ChatMessage request)
    {
        var conv_ID = request.Conversation_ID ?? Guid.NewGuid();
        var isNew = !_db.Set<ChatLogs>().Any(c => c.ID == conv_ID);

        var log = new ChatLogs()
        {
            ID = conv_ID,
            Role = RoleType.user,
            Content = request.Content,
            Title = isNew ? $"Conversation: {conv_ID.ToString().Substring(0,30)}" : null,
            TimeStamp = DateTime.UtcNow,
        };

        _db.Set<ChatLogs>().Add(log);
        _db.SaveChanges();

        return log;
    }

    public record ConversationSummary(Guid conversation_ID, string Title);

    /// <summary>
    /// Get a list of all conversations in the DB.
    /// </summary>
    /// <returns></returns>
    public List<ConversationSummary> GetConversations()
    {
        return _db.Set<ChatLogs>().Where(c => c.Title != null).Select(c=>new ConversationSummary(c.ID, c.Title)).Distinct().ToList();
    }

    /// <summary>
    /// Get all messages from an conversation
    /// </summary>
    /// <param name="id">Conversation ID</param>
    /// <param name="count">How many messages to fetch</param>
    /// <returns>A list of all the requested messages from newest to oldest</returns>
    public List<ChatLogs> GetMessages(Guid id, int? count)
    {
        var query = _db.Set<ChatLogs>().Where(c=>c.ID == id).OrderByDescending(c => c.TimeStamp);
        return (count.HasValue ? query.Take(count.Value) : query).OrderBy(c => c.TimeStamp).ToList();
    }
}