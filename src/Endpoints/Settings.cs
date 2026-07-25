using Yggdrasil.Services;

namespace Yggdrasil.Endpoints;

public static class Settings
{
    
    public static void MapSettingsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/settings/get-theme", (SettingsService service) => Results.Ok(service.getTheme()));
    }
}