// CharacterFactory.cs

using Yggdrasil.Models;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public class CharacterFactory{
    public static Character Create(AppDbContext db, Guid? world_ID = null){
        var character = AutoFaker.Generate<Character>();
        character.Worlds = [];
        
        // Set up character into the world, and give it the world ID if we passed a world ID
        if (world_ID != null){ 
            var world = db.Set<World>().Include(w => w.Characters).First(w => w.ID == world_ID);
            world.Characters.Add(character);
        }
        
        db.Set<Character>().Add(character);
        db.SaveChanges();
        return character;
    }

    public static Character Create(DatabaseFixture fixture, Guid? world_ID = null){
        using var db = fixture.CreateContext();
        return Create(db, world_ID);
    }
}
