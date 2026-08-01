/// World.cs

namespace Yggdrasil.Models;

public class World{
    public Guid ID {get; set;}
    public Guid? Lorebook_ID {get; set;}
    public required string Name {get; set;}
    public required string Description {get; set;}
    public string? PrefixInstruction {get; set;} // Occurs before NarratorInstruction
    public string? NarratorInstruction {get; set;} // occurs right after Prefix
    public string? SuffixInstruction {get; set;} // Occurs at the end of the entire prompt to narrator.
    public List<Character> Characters {get; set;} = [];
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}
