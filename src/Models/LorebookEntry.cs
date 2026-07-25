namespace Yggdrasil.Models;

public class LorebookEntry
{
    public Guid ID {get; set;}
    public String? Name {get;set;}
    public String? Content {get; set;}
    public List<String> Keywords {get; set;} = new();
}