// WorldController.cs
using Microsoft.AspNetCore.Mvc;
using Yggdrasil.DTO;
using Yggdrasil.Models;
using Yggdrasil.Services;
using Yggdrasil.Util;

namespace Yggdrasil.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorldController : ControllerBase
{
    private readonly WorldService _service;

    public WorldController(WorldService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public IActionResult GetAll([FromQuery] int? count) => ServiceResultExtensions.SafeExecute(() => _service.getWorlds(count));

    [HttpGet("{world_ID}")]
    public IActionResult get(Guid world_ID) => ServiceResultExtensions.SafeExecute(() => _service.getWorld(world_ID));

    [HttpPost("create")]
    public IActionResult Create([FromBody] WorldRequest request) => ServiceResultExtensions.SafeExecute(() => _service.createWorld(request));


    [HttpDelete("{world_ID}")]
    public async Task<IActionResult> Delete(Guid world_ID) => ServiceResultExtensions.SafeExecute(() => _service.DeleteWorld(world_ID));

    [HttpPost("{world_ID}/characters/{character_ID}")]
    public IActionResult AddCharacter(Guid world_ID, Guid character_ID) => ServiceResultExtensions.SafeExecute(() => _service.addCharacter(world_ID, character_ID));

    [HttpDelete("{world_ID}/characters/{character_ID}")]
    public IActionResult RemoveCharacter(Guid world_ID, Guid character_ID) => ServiceResultExtensions.SafeExecute(() => _service.removeCharacter(world_ID, character_ID));
}