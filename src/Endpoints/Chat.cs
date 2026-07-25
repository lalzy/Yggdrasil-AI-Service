using System.Data.Common;
using Yggdrasil.Data;
using Yggdrasil.Models;

namespace Yggdrasil.Endpoints;

public static class ChatEndpoints
{
    public static void MapConversationEndpoint(this WebApplication app)
    {
        app.MapPost("/api/chat", (AppDbContext db, ChatMessage request) =>
        {
            var convId = request.ConversationId ?? Guid.NewGuid();
            var isNew = !db.ChatLogs.Any(c => c.ConversationId == convId);

            var log = new ChatLogs()
            {
                ConversationId = convId,
                Role = RoleType.user,
                Content = request.Content,
                Title = isNew ? $"Conversation: {convId.ToString().Substring(0,30)}" : null,
                TimeStamp = DateTime.UtcNow
            };

            db.ChatLogs.Add(log);
            db.SaveChanges();

            return Results.Ok(log);
        });

        app.MapGet("/api/conversations", (AppDbContext db) =>
        {
            var conversations = db.ChatLogs
            .Where(c => c.Title != null)
            .Select(c => new {c.ConversationId, c.Title}).Distinct().ToList();

            return Results.Ok(conversations);
        });

        app.MapGet("/api/conversations/{id}/messages", (AppDbContext db, Guid id, int? count) =>
        {

            var query = db.ChatLogs.Where(c => c.ConversationId == id)
            .OrderByDescending(c => c.TimeStamp);

            var messages = (count.HasValue ? query.Take(count.Value) : query)
            .OrderBy(c => c.TimeStamp)
            .ToList();

            return Results.Ok(messages);
        });
    }
}


