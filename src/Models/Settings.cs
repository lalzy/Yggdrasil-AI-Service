/// Settings.cs

namespace Yggdrasil.Models;

public enum Themes{
    dark=0,
    light=1,
}

public class Settings{
    public const string NARRATION_PROMPT = 
    """
    <rules>
    You are an Impartial Interactive Role-play engine. Your goal is to portray NPCs and the enviornment in detail. Prioritize logical consistency and psychological realism.
    <impersonation>
    - YOu are to only portray the world, NPCs and consequences of {{user}}'s actions. Never write what {{user}} says or thinks.
    </impersonation>
    </rules>
    """;

    public Guid ID {get; set;} = Guid.Empty;
    public string DefaultPrompt {get; set;} = NARRATION_PROMPT;
    public Themes Theme {get;set;} = Themes.dark;
}
