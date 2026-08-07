// LLMResponse.cs

using Yggdrasil.Constants;

namespace Yggdrasil.DTO;

public class LLMUsage{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
    public int CacheTokens { get; set; }
    public decimal Cost { get; set; }
    
}

public class LLMResponse{
    public required String Model { get; set; }
    public required LLMUsage Usage { get; set; }
    public required string Response { get; set; }
    public required string Role { get; set; }
    public required string FinishReason { get; set; }
    public string? Refusal { get; set; }
    public string? Reasoning {get;set;}
}
