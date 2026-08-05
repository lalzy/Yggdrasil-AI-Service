// LLMServiceTests.cs

using Yggdrasil.Tests.Factories;
using Yggdrasil.Services;
using System.Xml.Linq;
using Yggdrasil.DTO;
using Yggdrasil.Models;
using Yggdrasil.Util;

using Xunit.Abstractions;

namespace Yggdrasil.Tests.Services;

public class LLMServiceTests : DatabaseTestBase{
    private readonly LLMService _service;
    private readonly Faker _faker = new();

    public LLMServiceTests(DatabaseFixture fixture) :base(fixture){
        _service = new LLMService(fixture.CreateContext());
    }

    // Helpers

    private (World, Character, Persona) CreateData(){
        var world = WorldFactory.Create(_fixture);
        var character = CharacterFactory.Create(_fixture, world.ID);
        var persona = PersonaFactory.Create(_fixture);
        world = _fixture.CreateContext().Set<World>().Include(w => w.Characters).First(w => w.ID == world.ID);
            return (world, character, persona);
    }
    
    private void CreateCharacters(int count = 1, Guid? world_ID = null){
        var context = _fixture.CreateContext();
        
        for(int i = 0; i < count; i++){
            CharacterFactory.Create(context, world_ID);
        }
    }

    [Fact]
    public void CreateLLMPayload_CorrectReturnType(){
        var (world, character, persona) = CreateData();
        var result = _service.CreateLLMPayload(world, persona);

        Assert.IsType<ServiceResult<LLMPayload>>(result);
    }

    [Fact]
    public void CreateLLMPayload_AlwaysCreateSystemMessageAsDefault(){
        var (world, character, persona) = CreateData();
        var payload = _service.CreateLLMPayload(world, persona).Data;
        world.IntroMessage = null;

        Assert.Single(payload.Messages);
        Assert.Equal("system", payload.Messages[0].Role);
        
    }

    [Fact]
    public void CreateLLMPayload_AlwaysSystemAndEmptyUserOnNoIntroMessage(){
        var (world, character, persona) = CreateData();
        var payload = _service.CreateLLMPayload(world, persona).Data!;

        Assert.Equal(2, payload.Messages!.Count);
        Assert.Equal("system", payload.Messages[0].Role);
        Assert.Equal("user", payload.Messages[1].Role);
    }
}

