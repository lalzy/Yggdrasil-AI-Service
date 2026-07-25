using Yggdrasil.Services;
using Yggdrasil.DTO;

namespace Yggdrasil.Endpoints;

public static class WorldEndpoints
{
    public static void MapWorldEndpoint(this WebApplication app)
    {
        app.MapGet("/api/get-worlds", (WorldService service, int? count) => Results.Ok(service.getWorlds(count)));
    }
}