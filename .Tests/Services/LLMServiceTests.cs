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

    
    private void checkUser(XDocument systemMessage, List<Character> characters){
        var charElements = systemMessage.Descendants("char").ToList();
        var skip = new[]{"Worlds","CreatedAt","UpdatedAt","Description","Name","ID" };
        var properties = typeof(Character).GetProperties().Where(p => !skip.Contains(p.Name)).ToList();

        foreach(var character in characters){
            foreach(var property in properties){
                bool found = charElements.Any(element => {
                    if(property.Name == "ExampleDialogue")
                        return character.ExampleDialogue!.All(line => element.Value.Contains(line));
                    return element.Value.Contains($"{property.GetValue(character)}");
                });
                Assert.True(found, $"Property '{property.Name}' not found in <char> element");
            }
        }
    }


    [Fact]
    public void CreateLLMPayload_CorrectReturnType(){
        var (world, character, persona) = CreateData();
        var result = _service.CreateLLMPayload(world, persona);

        Assert.IsType<ServiceResult<LLMPayload>>(result);
    }

    [Fact]
    public void CreateLLMPayload_NoCharacterInWorld(){
        var world = WorldFactory.Create(_fixture);
        var persona = PersonaFactory.Create(_fixture);
        var payload = _service.CreateLLMPayload(world, persona).Data;

        Assert.NotNull(payload);
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

        Assert.Equal(3, payload.Messages!.Count);
        Assert.Equal("system", payload.Messages[0].Role);
        Assert.Equal("user", payload.Messages[1].Role);
        Assert.Equal("assistant", payload.Messages[2].Role);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("Some string")]
    public void CreateLLMPayload_MessagesAdded(string? intro){
        var (world, character, persona) = CreateData();
        world.IntroMessage = intro;
        string content = _faker.Lorem.Lines();
        var message = new Message{Role="user", Content=content};
        var payload = _service.CreateLLMPayload(world, persona, [message]).Data!;
        
        // If no intro-message then first non-system message will be from the user.
        if(intro == null)
            Assert.Equivalent(message, payload.Messages![1]);
        else
            Assert.Equivalent(message, payload.Messages![3]);
    }

    [Fact]
    public void CreateLLMPayload_CharacterEntriesExist(){
        var (world, character, persona) = CreateData();
        var payload = _service.CreateLLMPayload(world, persona).Data!;
        XDocument systemMessage = XDocument.Parse(payload.Messages[0].Content);
        
        checkUser(systemMessage, [character]);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(5)]
    public void CreateLLMPayload_MultipleCharacterEntriesExist(int count){
        World world = WorldFactory.Create(_fixture);
        
        Persona persona = PersonaFactory.Create(_fixture);
        List<Character> characters = Enumerable.Range(0, count).Select(c => {
            Character character = CharacterFactory.Create(_fixture, world.ID);
            return character;
        }).ToList();
        
        world = _fixture.CreateContext().Set<World>().Include(w => w.Characters).First(w => w.ID == world.ID);
        var payload = _service.CreateLLMPayload(world, persona).Data!;
        
        checkUser(XDocument.Parse(payload.Messages[0].Content), characters);
    }
}

