// WorldServiceTests.cs

using Yggdrasil.Services;
using Bogus;
using AutoBogus;
using Yggdrasil.DTO;
using Yggdrasil.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Yggdrasil.Util;
using Yggdrasil.Tests.Factories;
using yggdrasil.Util;
using Xunit.Sdk;
using Yggdrasil.Data;

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

    private void CreateWorlds (int count=1)
    {
        for(int i = 0; i < count; i++)
        {
            WorldFactory.Create(_fixture);
        }
    }

    private List<Character> GetCharactersFromDB(Guid World_ID, AppDbContext? context = null)
    {
        context ??= _fixture.CreateContext();
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
    public void GetOne_GetRequested()
    {
        var world = WorldFactory.Create(_fixture);
        var fetch = _service.GetOne(world.ID).Data!;
        Assert.Equivalent(world, fetch);

        var fetched = _fixture.CreateContext().Set<World>().Include(w=>w.Characters).FirstOrDefault(w=>w.ID == world.ID);
        Assert.Equivalent(world, fetched);
    }

    [Fact]
    public void GetOne_GetCorrectWorldFromMany()
    {
        CreateWorlds(2);
        var world = WorldFactory.Create(_fixture);
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
        var request = AutoFaker.Generate<WorldRequest>();
        var convertedRequest = request.ConvertModelToDTO<World>();
        var world = _service.Create(request).Data!;
        var ID = world.ID;
        world.ID = Guid.Empty;

        Assert.Equivalent(convertedRequest, world, strict:false);

        // Check DB
        var fetched = _fixture.CreateContext().Set<Character>().FirstOrDefault(w=>w.ID == ID);

        Assert.Throws<EquivalentException>(()=>Assert.Equivalent(world, fetched));
    }

    [Fact]
    public void Creates_DefaultInstruction()
    {
        var request = AutoFaker.Generate<WorldRequest>();
        request.NarratorInstruction = null;
        var result = _service.Create(request).Data!;
        
        Assert.Equal(Yggdrasil.Models.Settings.NARRATION_PROMPT, result.NarratorInstruction);
    }

    [Fact]
    public void Deletes_DeletesTheWorld()
    {
        var context = _fixture.CreateContext();
        var world_ID = WorldFactory.Create(context).ID;

        _service.Delete(world_ID);

        // Verify deleted in DB
        var dbFetch = context.Set<World>().FirstOrDefault(w => w.ID == world_ID);
        Assert.Null(dbFetch);
    }

    [Fact]
    public void Deletes_OnlyRequestedWorldDeletes()
    {
        CreateWorlds(1);
        var world_ID = WorldFactory.Create(_fixture).ID;
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
        var world_ID = WorldFactory.Create(_fixture).ID;

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
        var world = WorldFactory.Create(_fixture);
        var character = CharacterFactory.Create(_fixture);
        
        Assert.Empty(GetCharactersFromDB(world.ID));
        _service.AddCharacter(world.ID, character.ID);

        // Verify in DB:
        Assert.NotEmpty(GetCharactersFromDB(world.ID));
    }

    [Fact]
    public void AddCharacter_InvalidWorldGuidThrows()
    {
        var character = CharacterFactory.Create(_fixture);
        Assert.Throws<KeyNotFoundException>(()=>_service.AddCharacter(_faker.Random.Guid(), character.ID));
    }

    [Fact]
    public void AddCharacter_InvalidCharacterGuidThrows()
    {
        var world = WorldFactory.Create(_fixture);
        Assert.Throws<KeyNotFoundException>(()=>_service.AddCharacter(world.ID, _faker.Random.Guid()));
    }

    [Fact]
    public void RemoveCharacter_CharacterIsRemoved()
    {
        var world = WorldFactory.Create(_fixture.CreateContext());
        var character = CharacterFactory.Create(_fixture.CreateContext(), world.ID);

        Assert.NotEmpty(GetCharactersFromDB(world.ID));
        _service.RemoveCharacter(world.ID, character.ID);
        Assert.Empty(GetCharactersFromDB(world.ID));
    }

    [Fact]
    public void RemoveCharacter_OnlyRequestedRemoved()
    {
        var context = _fixture.CreateContext();
        var world = WorldFactory.Create(context);
        int count = 3;

        var characters = Enumerable.Range(0, count).Select(i=>CharacterFactory.Create(context, world.ID)).ToList();
        Character characterToRemove = characters[_faker.Random.Int(0, count - 1)];

        Assert.Equal(GetCharactersFromDB(world.ID).Count, count);

        _service.RemoveCharacter(world.ID, characterToRemove.ID);

        var dbCharacters = GetCharactersFromDB(world.ID);
        Assert.Equal((count - 1), dbCharacters.Count);
    }

    [Fact]
    public void RemoveCharacter_InvalidWorldGuidThrows()
    {
        var context = _fixture.CreateContext();
        var world = WorldFactory.Create(context);
        var character = CharacterFactory.Create(context, world.ID);
        Assert.Throws<KeyNotFoundException>(()=>_service.RemoveCharacter(_faker.Random.Guid(), character.ID));
    }

    [Fact]
    public void RemoveCharacter_InvalidCharacterGuidThrows()
    {
        var world = WorldFactory.Create(_fixture);
        Assert.Throws<KeyNotFoundException>(()=>_service.RemoveCharacter(world.ID, _faker.Random.Guid()));
    }
}