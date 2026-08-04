// LLMSettings.cs

using System.Text.Json.Serialization;

namespace Yggdrasil.Models;

public enum APIType
{
    chatCompletion = 0
}

public class LLMConnection
{
    [JsonIgnore]
    public  Guid ID { get; set; }
    public required string Name { get; set; }
    public string? APIKey  { get; set; }
    public required string URL  { get; set; }
    public string? Model  { get; set; }
    public required APIType APIType  { get; set; }
    public bool Reasoning  { get; set; }
}
