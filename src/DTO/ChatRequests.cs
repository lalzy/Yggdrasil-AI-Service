// ChatRequests.cs

using Yggdrasil.Models;

namespace Yggdrasil.DTO;

public class SendRequest{
    public LLMConnection Connection { get; set; }
    public LLMPayload Payload { get; set; }
}
