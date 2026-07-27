using Bogus;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Data;
using Yggdrasil.Models;
namespace Yggdrasil.Tests.Utility;

public static class Factory
{

    public static World CreateWorld(AppDbContext db, string? instruction = null)
    {
        Faker faker = new();
        var world = new World
        {
            Name = faker.Lorem.Lines(),
            Description = faker.Lorem.Lines(),
            NarratorInstruction = instruction,
        };
        db.Set<World>().Add(world);
        db.SaveChanges();
        return world;
    }

    public static World CreateWorld(DatabaseFixture fixture, string? instruction = null)
    {
        using var db = fixture.CreateContext();
        return CreateWorld(db, instruction);
    }

    public static Character CreateCharacter(AppDbContext db, Guid? world_ID = null)
    {
        Faker faker = new();
        var character = new Character
        {
            Name = faker.Name.FullName(),
            Description = faker.Lorem.Lines(),
            Occupation = faker.Lorem.Lines(),
            NarrativeRole = faker.Lorem.Lines(),
            Appearance = faker.Lorem.Lines(),
            Personality = faker.Lorem.Lines(),
            Gender = faker.Lorem.Lines(),
            Race = faker.Lorem.Lines(),
            Equipment = faker.Lorem.Lines(),
        };
        // Set up character into the world, and give it the world ID if we passed a world ID
        if (world_ID != null){ 
            var world = db.Set<World>().Include(w=>w.Characters).First(w=>w.ID == world_ID);
            world.Characters.Add(character);
            character.World_IDs.Add(world_ID.Value);
        }
        db.Set<Character>().Add(character);
        db.SaveChanges();
        return character;
    }

    public static Character CreateCharacter(DatabaseFixture fixture, Guid? world_ID = null)
    {
        using var db = fixture.CreateContext();
        return CreateCharacter(db, world_ID);
    }
}