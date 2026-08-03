// ChatController.cs
using Microsoft.AspNetCore.Mvc;

using Yggdrasil.DTO;
using Yggdrasil.Services;
using Yggdrasil.Util;

namespace Yggdrasil.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LLMController : ControllerBase{
    private readonly LLMService _service;

    public LLMController(LLMService service){
        _service = service;
    }

    [HttpGet("system-prompt/{world_ID}/{persona_ID}")]
    public IActionResult Get(Guid world_ID, Guid persona_ID) => ServiceResultExtensions.SafeExecute(() => _service.GetPromptString(world_ID, persona_ID));
}
