using Yggdrasil.Services;
using Yggdrasil.DTO;

namespace Yggdrasil.Endpoints;

public static class ChatEndpoints
{
    public static void MapConversationEndpoint(this WebApplication app)
    {
        app.MapPost("/api/chat", (ChatService service, ChatMessage request) => Results.Ok(service.CreateMessage(request)));
        app.MapGet("/api/conversations", (ChatService service) => Results.Ok(service.GetConversations()));
        app.MapGet("/api/conversations/{ID}/messages", (ChatService service, Guid ID, int? count) => Results.Ok(service.GetMessages(ID, count)));
    }
}


