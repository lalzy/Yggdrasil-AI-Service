// WorldRequest.cs
using Yggdrasil.Models;

namespace Yggdrasil.DTO;

public class WorldRequest
{
    public Guid Lorebook_ID {get; set;}
    public string? Name {get; set;}
    public string? Description {get; set;}
    public string? NarratorInstruction {get; set;}
    public List<Character> Characters {get; set;} = new();
}