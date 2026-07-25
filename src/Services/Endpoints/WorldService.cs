using Microsoft.EntityFrameworkCore;
using Yggdrasil.Models;
using Yggdrasil.Util;
using Yggdrasil.DTO;

namespace Yggdrasil.Services;


public class WorldService
{
    private readonly AppDbContext _db;
    public WorldService(AppDbContext db)
    {
        _db = db;
    }

    public ServiceResult<World> getWorld(Guid world_ID)
    {
        var world = _db.Set<World>().Where(w=>w.ID == world_ID).Distinct().FirstOrDefault();
        if (world == null) return new ServiceResult<World>(null, ErrorMessages.WORLD_NOT_EXIST);
        return new ServiceResult<World>(world);
    }

    public record WorldSummary(Guid world_ID, string Name, string DEscription);
    
    public List<WorldSummary> getWorlds(int? count)
    {
        return _db.Set<World>().Where<World>(w=>w.Name != null).Select(w=> new WorldSummary(w.ID, w.Name, w.Description)).Distinct().ToList();
    }

    public ServiceResult<World> createWorld(string name,  string? description=null, string? narratorInstructions=null)
    {
        var world =  new World
        {
            Name = name,
            Description = description,
            NarratorInstruction = (narratorInstructions != null) ? narratorInstructions : _db.Set<Settings>().First().DefaultPrompt
        };

        _db.Set<World>().Add(world);
        _db.SaveChanges();

        return new ServiceResult<World>(world);
    }

    public ServiceResult<bool> DeleteWorld(Guid world_ID)
    {
        var world = _db.Set<World>().Include(w=>w.Characters).FirstOrDefault(w=>w.ID == world_ID);
        if (world == null) return new ServiceResult<bool>(false, ErrorMessages.WORLD_NOT_EXIST);
        world.Characters.Clear();
        _db.Set<World>().Remove(world);
        _db.SaveChanges();

        return new ServiceResult<bool>(true);
    }

    public ServiceResult<Character> addCharacter(Guid world_ID, Character character)
    {
        var world = _db.Set<World>().Where(w => w.ID == world_ID).Distinct().Include(w=>w.Characters).First();
        world.Characters.Add(character);

        _db.SaveChanges();
        return new ServiceResult<Character>(character);
    }

    public ServiceResult<bool> removeCharacter(Guid world_ID, Guid character_ID)
    {
        var world = _db.Set<World>().Include(w=>w.Characters).FirstOrDefault(w=>w.ID == world_ID);
        if (world == null) return new ServiceResult<bool>(false, ErrorMessages.WORLD_NOT_EXIST);
        var character = world.Characters.FirstOrDefault(c=>c.ID == character_ID);
        if(character == null) return new ServiceResult<bool>(false, ErrorMessages.CHARACTER_NOT_IN_WORLD);
        world.Characters.Remove(character);
        _db.SaveChanges();

        return new ServiceResult<bool>(true);
    }
}