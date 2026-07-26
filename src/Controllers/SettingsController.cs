// SettingsController.cs
using Microsoft.AspNetCore.Mvc;
using Yggdrasil.Services;

namespace Yggdrasil.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly SettingsService _service;
    public SettingsController(SettingsService service)
    {
        _service = service;
    }
    
    [HttpGet("get-theme")]
    public IActionResult getAll()
    {
        return Ok(_service.getTheme());
    }

    [HttpGet("/api/debug/settings")]
    public IActionResult getAllSettings()
    {
        return Ok(_service.getAll());
    }
}
