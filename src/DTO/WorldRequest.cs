// WorldRequest.cs
using System.ComponentModel.DataAnnotations;
using Yggdrasil.Models;

namespace Yggdrasil.DTO;

public class WorldRequest
{
    public Guid? Lorebook_ID {get; set;}
    [Required]
    [MinLength(1)]
    public required string Name {get; set;}
    [Required]
    [MinLength(1)]
    public required string Description {get; set;}
    public string? PrefixInstruction {get; set;} // Occurs before NarratorInstruction
    public string? NarratorInstruction {get; set;} // occurs right after Prefix
    public string? SuffixInstruction {get; set;} // Occurs at the end of the entire prompt to narrator.
    public List<Character> Characters {get; set;} = new();
}