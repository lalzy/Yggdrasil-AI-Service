// LLMRoles.cs

namespace Yggdrasil.Constants;

public  class LLMRoles{
    public const string User = "user";
    public const string System = "system";
    public const string Assistant = "assistant";

    public static string FromString(string role) => role.ToLower() switch
    {
        "user" => User,
        "system" => System,
        "assistant" => Assistant,
    };
}
