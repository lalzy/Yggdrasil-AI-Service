// LLMPayload.cs

namespace Yggdrasil.DTO;

public class Message{
    public required string Role { get; set; }
    public required string Content { get; set; }
}

public class LLMPayload{
    public List<Message>? Messages { get; set; } = [];
}
