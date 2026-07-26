/// Lorebook.cs

namespace Yggdrasil.Models;

public class Lorebook
{
    public Guid ID {get; set;}
    public List<Guid> World_IDs {get; set;} = new();
    public String? Name {get;set;}
}