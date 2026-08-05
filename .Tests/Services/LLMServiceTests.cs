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


    // Tests

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

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void CreateLLMPayload_CharactersInSystemMessage(int characterCount){
        World world = WorldFactory.Create(_fixture);
        Persona persona = PersonaFactory.Create(_fixture);
        List<Character> characters = Enumerable.Range(0, characterCount).Select(c => CharacterFactory.Create(_fixture, world.ID)).ToList();
        world.Characters = characters;

        var payload = _service.CreateLLMPayload(world, persona).Data!;
        var systemMessage = XDocument.Parse(payload.Messages[0].Content);
        
        var skip = new[]{ "Worlds","CreatedAt","UpdatedAt","Description","Name","ID" }; // Doesn't exist in the XML by intent
        var properties = typeof(Character).GetProperties().Where(p => !skip.Contains(p.Name)).ToList();

        // Check that the character data does indeed exist as XML Entry
        foreach(var character in characters){
            foreach(var property in properties){
                bool found = systemMessage.Descendants("char").ToList().Any(element => {
                    if(property.Name == "ExampleDialogue")
                        return character.ExampleDialogue!.All(line => element.Value.Contains(line));
                    return element.Value.Contains($"{property.GetValue(character)}");
                });
                Assert.True(found, $"Property '{property.Name}' not found in any <char> element");
            }
        }
    }
}

