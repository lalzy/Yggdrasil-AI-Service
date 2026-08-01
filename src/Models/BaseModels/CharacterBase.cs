// CharacterBase.cs

namespace Yggdrasil.Models;

public abstract class CharacterBase{
    public Guid ID {get; set;}
    public required string Name {get; set;}
    public required string Gender {get; set;}
    public string? Race {get; set;}
    public string? Occupation {get; set;}
    public string? Appearance {get; set;}
    public string? Equipment {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}
