/// MemoryEntry.cs

namespace Yggdrasil.Models;

public class MemoryEntry
{
    public Guid ID {get; set;}
    public Guid Conversation_ID {get; set;}
    public string? Content {get; set;}
    public DateTime CreatedAt {get; set;}
    public List <string> Keywords {get; set;} = new();
}