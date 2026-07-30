/// Character.cs

namespace Yggdrasil.Models;

public class Character  : CharacterBase 
{
    public List<World> Worlds {get; set;} = [];
    public string? NarrativeRole {get; set;}
    public required string Personality {get; set;}
    public string? ExampleDialogue {get; set;}
    public required string Description {get; set;}
}