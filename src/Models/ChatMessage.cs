namespace AIService.Models;

public class ChatMessage
{
    public Guid? ConversationId {get; set;}
    public string Content {get; set;} = "";
    public string Title {get; set;} = "";
}