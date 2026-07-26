/// Character.cs

namespace Yggdrasil.Models;

public class Character
{
    public Guid ID {get; set;}
    public List<Guid> World_IDs {get; set;} = new();
    public String? Name {get; set;}
    public String? Occupation {get; set;}
    public String? NarrativeRole {get; set;}
    public String? Appearance {get; set;}
    public String? Personality {get; set;}
    public String? Gender {get; set;}
    public String? Race {get; set;}
    public String? Equipment {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
}