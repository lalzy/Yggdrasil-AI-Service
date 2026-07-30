// CharacterServiceTests.cs
using Bogus;
using AutoBogus;
using Yggdrasil.Tests.Factories;
using Yggdrasil.Services;
using Yggdrasil.Models;
using Yggdrasil.DTO;
using Yggdrasil.Util;
using yggdrasil.Util;
using Xunit.Sdk;

namespace Yggdrasil.Tests.Services;

public class CharacterServiceTests :DatabaseTestBase
{
    private readonly CharacterService _service;
    private readonly Faker _faker = new ();

    public CharacterServiceTests(DatabaseFixture fixture) : base(fixture)
    {
        _service = new CharacterService(fixture.CreateContext());
    }

    // Helpers
    private void CreateCharacters(int count = 1, Guid? world_ID = null)
    {
        var context = _fixture.CreateContext();
        for(int i = 0; i < count; i++)
        {
            CharacterFactory.Create(context, world_ID);
        }
    } 
    
    private Character? GetCharacterFromDB(Guid characet_ID)
    {
        var context = _fixture.CreateContext();
        return context.Set<Character>().FirstOrDefault(c => c.ID == characet_ID);
    }

    [Fact]
    public void GetAll_GetAllMade()
    {
        int count = _faker.Random.Int(20, 30);
        CreateCharacters(count);

        var fetch = _service.GetAll().Data!;

        Assert.Equal(count, fetch.Count);
    }

    [Fact]
    public void GetAll_GetOnlyRequestedAmount()
    {
        int count = 20;
        int toGet = 5;
        CreateCharacters(count);
        var fetch = _service.GetAll(toGet).Data!;

        Assert.Equal(toGet, fetch.Count);
    }

    [Fact]
    public void GetAll_ReturnsCorrectServiceResultType()
    {
        CreateCharacters(3);
        var fetch = _service.GetAll();

        Assert.IsType<ServiceResult<List<CharacterSummary>>>(fetch);
    }

    [Fact]
    public void GetAll_EmptyReturnsEmpty(){
        var fetch = _service.GetAll().Data!;

        Assert.Empty(fetch);
    }

    [Fact]
    public void GetAll_ThrowsOnInvalidCount()
    {
        CreateCharacters(3);
        Assert.Throws<ArgumentException>(()=>_service.GetAll(-1));
    }

    [Fact]
    public void GetOne_GetRequested()
    {
        var character = CharacterFactory.Create(_fixture.CreateContext());
        var fetch = _service.GetOne(character.ID).Data!;

        Assert.Equivalent(character, fetch);
    }

    [Fact]
    public void GetOne_GetCorrectFromMany()
    {
        var context = _fixture.CreateContext();
        var characterNotToGet = CharacterFactory.Create(context);
        var characterToGet = CharacterFactory.Create(context);

        var fetch = _service.GetOne(characterToGet.ID).Data!;

        Assert.Equivalent(characterToGet, fetch);
        Assert.Throws<EquivalentException>(()=> Assert.Equivalent(characterNotToGet, fetch));
    }

    [Fact]
    public void GetOne_CorrectReturnType(){
        var character = CharacterFactory.Create(_fixture);

        var fetch = _service.GetOne(character.ID);

        Assert.IsType<ServiceResult<Character>>(fetch);
    }

    [Fact]
    public void GetOne_InvalidGuidThrows()
    {
        CreateCharacters(2);
        Assert.Throws<KeyNotFoundException>(()=>_service.GetOne(_faker.Random.Uuid()));
    }

    [Fact]
    public void Create_Success()
    {
        var request = AutoFaker.Generate<CharacterRequest>();
        var convertedRequest = request.ConvertModelToDTO<Character>();
        var character = _service.Create(request).Data!;
        var ID = character.ID;
        character.ID = Guid.Empty; // Request doesn't have ID.
        Assert.Equivalent(convertedRequest, character, strict:false);

        character.ID = ID; // add ID back for DB Check
        // Check for DB:
        var fetched = _fixture.CreateContext().Set<Character>().FirstOrDefault(c=>c.ID == ID);
        Assert.Equivalent(character, fetched);
    }

    [Fact]
    public void Create_ReturnType(){
        var request = AutoFaker.Generate<CharacterRequest>();
        var ret = _service.Create(request);

        Assert.IsType<ServiceResult<Character>>(ret);
    }

    [Fact]
    public void Delete_DeletesTheCharacter()
    {
        Character character = CharacterFactory.Create(_fixture);
        Character dbCharacter = GetCharacterFromDB(character.ID);

        Assert.Equivalent(
            character,
            dbCharacter
        );

        _service.Delete(character.ID);
        dbCharacter = GetCharacterFromDB(character.ID);
        Assert.Null(dbCharacter);
    }

    [Fact]
    public void Delete_DeletesOnlySelectCharacter()
    {
        Character character = CharacterFactory.Create(_fixture);
        CreateCharacters(2);

        _service.Delete(character.ID);
        var dbCharacter = GetCharacterFromDB(character.ID);
        var totalRemaining = _fixture.CreateContext().Set<Character>().Count();

        Assert.Null(dbCharacter);
        Assert.Equal(2, totalRemaining);
    }

    [Fact]
    public void Delete_CorrectReturn(){
        
        Character character = CharacterFactory.Create(_fixture);

        var ret = _service.Delete(character.ID);
        Assert.IsType<ServiceResult<bool>>(ret);
        Assert.True(ret.Data!);
    }

    [Fact]
    public void Delete_InvalidGuidThrows(){
        Assert.Throws<KeyNotFoundException>(() => _service.Delete(_faker.Random.Guid()));
    }
}
