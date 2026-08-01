/// MemoryEntry.cs

namespace Yggdrasil.Models;

public class MemoryEntry{
    public Guid ID {get; set;}
    public required Guid Conversation_ID {get; set;}
    public required string Content {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
    public List <string> Keywords {get; set;} = new();
}
