/// Settings.cs

namespace Yggdrasil.Models;

public enum Themes
{
    dark=0,
    light=1,
}
public class Settings
{
    const string NARRATION_PROMPT = "You are an narrator!";
    public Guid ID {get; set;}
    public string DefaultPrompt {get; set;} = NARRATION_PROMPT;
    public Themes Theme {get;set;} = Themes.dark;
}