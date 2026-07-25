namespace Yggdrasil.Models;

public class World
{
    public Guid ID {get; set;}
    public string? Name {get; set;}
    public string? Description {get; set;}
    public string? NarratorInstruction {get; set;}
    public List<Character> Characters {get; set;} = new();
}