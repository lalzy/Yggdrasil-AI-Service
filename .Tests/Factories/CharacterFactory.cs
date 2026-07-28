// CharacterFactory.cs

using AutoBogus;
using Yggdrasil.DTO;
using Yggdrasil.Models;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public class CharacterFactory{
    public static Character CreateCharacter(AppDbContext db, Guid? world_ID = null)
    {
        var character = AutoFaker.Generate<Character>();
        
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