using System.Data.Common;
using Yggdrasil.Models;

namespace Yggdrasil.Services;

public class ChatService{
    private readonly AppDbContext _db;
    public ChatService(AppDbContext db)
    {
        _db = db;
    }

    public ChatLogs CreateMessage (ChatMessage request)
    {
        var conv_ID = request.Conversation_ID ?? Guid.NewGuid();
        var isNew = !_db.ChatLogs.Any(c => c.Conversation_ID == conv_ID);

        var log = new ChatLogs()
        {
            Conversation_ID = conv_ID,
            Role = RoleType.user,
            Content = request.Content,
            Title = isNew ? $"Conversation: {conv_ID.ToString().Substring(0,30)}" : null,
            TimeStamp = DateTime.UtcNow,
        };

        _db.ChatLogs.Add(log);
        _db.SaveChanges();

        return log;
    }

    public record ConversationSummary(Guid conversation_ID, string Title);

    public List<ConversationSummary> GetConversations()
    {
        return _db.ChatLogs.Where(c => c.Title != null).Select(c=>new ConversationSummary(c.Conversation_ID, c.Title)).Distinct().ToList();
    }

    public List<ChatLogs> GetMessages(Guid id, int? count)
    {
        var query = _db.ChatLogs.Where(c=>c.Conversation_ID == id).OrderByDescending(c => c.TimeStamp);
        return (count.HasValue ? query.Take(count.Value) : query).OrderBy(c => c.TimeStamp).ToList();
    }
}