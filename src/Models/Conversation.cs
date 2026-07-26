/// Conversation.cs

using Yggdrasil.Services;

namespace Yggdrasil.Models;


public class Conversation
{
    public Guid ID {get; set;}
    public Guid World_ID {get; set;}
    public string? Title {get; set;}
    public List<ChatMessage> ChatMessages {get;set;} = new();
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
}