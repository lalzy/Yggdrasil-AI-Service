// Records.cs

namespace Yggdrasil.DTO;

public record WorldSummary(Guid world_ID, string Name, string Description);
public record CharacterSummary(Guid charactter_ID, string Name, string Description);
public record PersonaSummary(Guid persona_ID, string Name, string Description);
