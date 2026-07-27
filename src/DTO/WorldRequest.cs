// WorldRequest.cs
using System.ComponentModel.DataAnnotations;
using Yggdrasil.Models;

namespace Yggdrasil.DTO;

public class WorldRequest
{
    public Guid? Lorebook_ID {get; set;}
    [Required]
    [MinLength(1)]
    public string? Name {get; set;}
    [Required]
    [MinLength(1)]
    public string? Description {get; set;}
    public string? NarratorInstruction {get; set;}
    public List<Character> Characters {get; set;} = new();
}