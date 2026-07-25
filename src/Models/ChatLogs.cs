namespace Yggdrasil.Models;

public enum RoleType
{
    system=0,
    assistant=1,
    user=2,
}

public class ChatLogs()
{
    public int Id {get; set;}
    public Guid ConversationId {get; set;}
    public RoleType Role {get; set;}
    public string Content {get; set;} = "";
    public string? Title {get; set;}
    public DateTime TimeStamp {get; set;}

}