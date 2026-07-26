// ChatMessageRequest.cs
namespace Yggdrasil.DTO;

public class ChatMessage
{
    public Guid Conversation_ID {get; set;}
    public Guid World_ID {get; set;}
    public string Content {get; set;} = "";
    public string Title {get; set;} = "";
}