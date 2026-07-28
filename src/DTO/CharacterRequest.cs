// CharacterRequest.cs

using System.ComponentModel.DataAnnotations;

namespace Yggdrasil.DTO;

public class CharacterRequest
{
    [Required]
    [MinLength(1)]
    public required string Name {get; set;}
    [Required]
    [MinLength(1)]
    public required string Description {get; set;}
    [Required]
    [MinLength(1)]
    public required string Personality {get; set;}
    [Required]
    [MinLength(1)]
    public required string Gender {get; set;}
    public string? Race {get; set;}
    public string? Occupation {get; set;}
    public string? Appearance {get; set;}
    public string? Equipment {get; set;}
    public string? ExampleDialogue {get; set;}
    public string? NarrativeRole {get; set;}
    
}