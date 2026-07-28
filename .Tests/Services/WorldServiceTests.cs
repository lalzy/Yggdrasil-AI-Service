// WorldServiceTests.cs

using Yggdrasil.Services;
using Bogus;
using Yggdrasil.DTO;
using Yggdrasil.Models;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Tests.Utility;
using System.Runtime.InteropServices;
using Yggdrasil.Util;

namespace Yggdrasil.Tests.Services;

public class WorldServiceTests : DatabaseTestBase
{
    private readonly WorldService _service;
    private readonly Faker _faker = new();

    public WorldServiceTests(DatabaseFixture fixture) : base(fixture)
    {
        _service = new WorldService(fixture.CreateContext());
    }
    // Helpers

    private WorldRequest CreateRequest(string? NarratorInstruction = null)
    {
        return new WorldRequest
        {
            Name = _faker.Name.FullName(),
            Description = _faker.Lorem.Lines(),
            NarratorInstruction = NarratorInstruction,
        };
    }

    private void CreateWorlds (int count=1)
    {
        for(int i = 0; i < count; i++)
        {
            _service.Create(CreateRequest());
        }
    }

    private List<Character> GetCharactersFromDB(Guid World_ID)
    {
        var context = _fixture.CreateContext();
        return context.Set<World>().Include(w => w.Characters).First(w => w.ID == World_ID).Characters;
    }

    // Tests
    [Fact]
    public void GetAll_GetAllWorldsMade()
    {
        int count = _faker.Random.Int(20,30);
        CreateWorlds(count);
        var fetched = _service.GetAll().Data;
        Assert.Equal(count, fetched.Count);
    }

    [Fact]
    public void GetAll_GetOnlyRequestedAmount()
    {
        int count = 10;
        int toGet = 3;
        CreateWorlds(count);

        var fetched = _service.GetAll(toGet).Data;
        Assert.Equal(toGet, fetched.Count);
    }

    [Fact]
    public void GetAll_ReturnsCorrectServiceResultType()
    {
        CreateWorlds(3);
        var fetch = _service.GetAll();

        Assert.IsType<ServiceResult<List<WorldSummary>>>(fetch);
    }

    [Fact]
    public void GetAll_LessThanOneCountThrows()
    {
        CreateWorlds(3);
        Assert.Throws<ArgumentException>(()=>_service.GetAll(-1));
    }

    [Fact]
    public void GetOne_Success()
    {
        var world = _service.Create(CreateRequest(_faker.Lorem.Lines())).Data!;
        var fetch = _service.GetOne(world.ID).Data!;
        Assert.Equivalent(world, fetch);
    }

    [Fact]
    public void GetOne_GetCorrectWorldFromMany()
    {
        CreateWorlds(2);
        var world = _service.Create(CreateRequest(_faker.Lorem.Lines())).Data!;
        CreateWorlds(2);
        var fetch = _service.GetOne(world.ID).Data!;
        Assert.Equivalent(world, fetch);
    }

    [Fact]
    public void GetOne_InvalidWorldGuidThrows()
    {
        Assert.Throws<KeyNotFoundException>(()=>_service.GetOne(_faker.Random.Guid()));
    }

    [Fact]
    public void Creates_Success()
    {
        var request = CreateRequest("someCustomInstruction");
        var result = _service.Create(request).Data;

        Assert.Equivalent(request, result);
    }

    [Fact]
    public void Creates_DefaultInstruction()
    {
        var result = _service.Create(CreateRequest()).Data;

        Assert.Equal(Yggdrasil.Models.Settings.NARRATION_PROMPT, result.NarratorInstruction);
    }

    [Fact]
    public void Deletes_DeletesTheWorld()
    {
        var world_ID = _service.Create(CreateRequest()).Data!.ID;

        Assert.True(_service.GetOne(world_ID).Data != null);

        _service.Delete(world_ID);

        // Verify deleted
        var context = _fixture.CreateContext();
        var dbFetch = context.Set<World>().FirstOrDefault(w => w.ID == world_ID);
        Assert.Null(dbFetch);
    }

    [Fact]
    public void Deletes_OnlyRequestedWorldDeletes()
    {
        CreateWorlds(1);
        var world_ID = _service.Create(CreateRequest()).Data!.ID;
        CreateWorlds(1);
        Assert.Equal(3, _service.GetAll().Data!.Count);
        _service.Delete(world_ID);
        var fetch = _service.GetAll().Data!;
        Assert.Equal(2, fetch.Count);
        Assert.DoesNotContain(fetch, w=>w.world_ID == world_ID);
    }

    [Fact]
    public void Deletes_DeletedWorldReturnTrue()
    {
        var world_ID = _service.Create(CreateRequest()).Data!.ID;
        var fetch = _service.Delete(world_ID);
        Assert.True(fetch.Data);
    }

    [Fact]
    public void Deletes_InvalidGuidThrows()
    {
        CreateWorlds(1);
        Assert.Throws<KeyNotFoundException>(()=>_service.Delete(_faker.Random.Guid()));
    }

    [Fact]
    public void AddCharacter_CharacterIsAdded()
    {
        var result = _service.Create(CreateRequest()).Data!;
        var character = Factory.CreateCharacter(_fixture);

        
        Assert.Empty(GetCharactersFromDB(result.ID));
        _service.AddCharacter(result.ID, character.ID);

        Assert.NotEmpty(GetCharactersFromDB(result.ID));
    }

    [Fact]
    public void AddCharacter_InvalidWorldGuidThrows()
    {
        var result = _service.Create(CreateRequest()).Data!;
        var character = Factory.CreateCharacter(_fixture, result.ID);
        Assert.Throws<KeyNotFoundException>(()=>_service.AddCharacter(_faker.Random.Guid(), character.ID));
    }

    [Fact]
    public void AddCharacter_InvalidCharacterGuidThrows()
    {
        var result = _service.Create(CreateRequest()).Data!;
        Assert.Throws<KeyNotFoundException>(()=>_service.AddCharacter(result.ID, _faker.Random.Guid()));
    }

    [Fact]
    public void RemoveCharacter_CharacterIsRemoved()
    {
        var result = _service.Create(CreateRequest()).Data!;
        var character = Factory.CreateCharacter(_fixture, result.ID);

        Assert.NotEmpty(GetCharactersFromDB(result.ID));
        _service.RemoveCharacter(result.ID, character.ID);
        Assert.Empty(GetCharactersFromDB(result.ID));
    }

    [Fact]
    public void RemoveCharacter_InvalidWorldGuidThrows()
    {
        var world_ID = _service.Create(CreateRequest()).Data!.ID;
        var character = Factory.CreateCharacter(_fixture, world_ID);
        Assert.Throws<KeyNotFoundException>(()=>_service.RemoveCharacter(_faker.Random.Guid(), character.ID));
    }

    [Fact]
    public void RemoveCharacter_InvalidCharacterGuidThrows()
    {
        var world_ID = _service.Create(CreateRequest()).Data!.ID;
        Assert.Throws<KeyNotFoundException>(()=>_service.RemoveCharacter(world_ID, _faker.Random.Guid()));
    }
}