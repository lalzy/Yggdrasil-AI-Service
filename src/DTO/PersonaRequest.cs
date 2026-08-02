// PersonaRequest.cs

using System.ComponentModel.DataAnnotations;

namespace Yggdrasil.DTO;

public class PersonaRequest{
    [Required]
    [MinLength(1)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    public required string Description { get; set; }
    [Required]
    [MinLength(1)]
    public required string Gender { get; set; }
    public string? Race { get; set; }
    public string? Apperance { get; set; }
    public string? Equipment { get; set; }
}
