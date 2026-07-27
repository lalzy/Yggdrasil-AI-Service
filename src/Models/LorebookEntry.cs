/// LorebookEntry.cs

namespace Yggdrasil.Models;

public class LorebookEntry
{
    public Guid ID {get; set;}
    public Guid Lorebook_ID {get; set;}
    public required string Name {get;set;}
    public required string Content {get; set;}
    public List<string> Keywords {get; set;} = [];
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}