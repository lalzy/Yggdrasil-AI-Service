using Bogus;
using Yggdrasil.Models;
namespace Yggdrasil.Tests.Helpers;

public static class CreateHelpers
{
    public static Character CreateCharacter(DatabaseFixture fixture, Guid? world_ID)
    {
        Faker faker = new();
        using var db = fixture.CreateContext();
        var character = new Character
        {
            Name = faker.Name.FullName(),
            Occupation = faker.Lorem.Lines(),
            NarrativeRole = faker.Lorem.Lines(),
            Appearance  = faker.Lorem.Lines(),
            Personality  = faker.Lorem.Lines(),
            Gender  = faker.Lorem.Lines(),
            Race  = faker.Lorem.Lines(),
            Equipment = faker.Lorem.Lines(),
        };
        if(world_ID != null) character.World_IDs.Add(world_ID.Value);
        db.Set<Character>().Add(character);
        db.SaveChanges();
        return character;
    }
}