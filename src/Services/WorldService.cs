// WorldService.cs
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Models;
using Yggdrasil.Util;
using Yggdrasil.Data;

using Yggdrasil.DTO;
using SQLitePCL;
namespace Yggdrasil.Services;


public class WorldService
{
    private readonly AppDbContext _db;
    public WorldService(AppDbContext db)
    {
        _db = db;
    }

    public record WorldSummary(Guid world_ID, string Name, string Description);
    /// <summary>
    /// Get all world as a SummaryRecord.
    /// </summary>
    /// <param name="count">How many records to fetch</param>
    /// <returns>All word records as a list</returns>
    public ServiceResult<List<WorldSummary>> GetWorlds(int? count=null)
    {
        var query = _db.Set<World>().Where<World>(w=>w.Name != null).Select(w=> new WorldSummary(w.ID, w.Name, w.Description)).Distinct();
        if(count.HasValue){ 
            if(count < 1) throw new ArgumentException("Less than 1 requested"); 
            query = query.Take(count.Value);
        }
        return new(query.ToList());
    }

    /// <summary>
    /// Get the requested world object
    /// </summary>
    /// <param name="world_ID"></param>
    /// <returns>World Object or null with error text.</returns>
    public ServiceResult<World> GetWorld(Guid world_ID)
    {
        var world = _db.Set<World>().Where(w=>w.ID == world_ID).Distinct().FirstOrDefault();
        if (world == null) throw new KeyNotFoundException(ErrorMessages.WORLD_NOT_EXIST);
        return new ServiceResult<World>(world);
    }

    /// <summary>
    /// Create a new World
    /// </summary>
    /// <param name="name">Name of the World</param>
    /// <param name="description">Description of the world</param>
    /// <param name="narratorInstructions">A custom instruction if desired</param>
    /// <returns>World object</returns>
    public ServiceResult<World> CreateWorld(WorldRequest request)
    {
        var world =  new World
        {
            Name = request.Name,
            Description = request.Description,
            NarratorInstruction = (request.NarratorInstruction != null) ? request.NarratorInstruction : _db.Set<Yggdrasil.Models.Settings>().First().DefaultPrompt
        };

        _db.Set<World>().Add(world);
        _db.SaveChanges();

        return new ServiceResult<World>(world);
    }

    /// <summary>
    /// Delete a world
    /// </summary>
    /// <param name="world_ID"></param>
    /// <returns>Boolean true if successfull, false with error if not</returns>
    public ServiceResult<bool> DeleteWorld(Guid world_ID)
    {
        var world = _db.Set<World>().Include(w=>w.Characters).FirstOrDefault(w=>w.ID == world_ID);
        if (world == null) throw new KeyNotFoundException(ErrorMessages.WORLD_NOT_EXIST);
        world.Characters.Clear();
        _db.Set<World>().Remove(world);
        _db.SaveChanges();

        return new ServiceResult<bool>(true);
    }

    /// <summary>
    /// Add a character to the world.
    /// </summary>
    /// <param name="world_ID"></param>
    /// <param name="character_ID">ID of character to add to the world</param>
    /// <returns>The Character</returns>
    public ServiceResult<Character> AddCharacter(Guid world_ID, Guid character_ID)
    {
        var world = _db.Set<World>().Where(w => w.ID == world_ID).Distinct().FirstOrDefault();
        if(world == null) throw new KeyNotFoundException(ErrorMessages.WORLD_NOT_EXIST);
        var character = _db.Set<Character>().Where(c=>c.ID == character_ID).Distinct().FirstOrDefault();
        if(character == null) throw new KeyNotFoundException(ErrorMessages.CHARACTER_NOT_EXIST);

        world.Characters.Add(character);

        _db.SaveChanges();
        return new ServiceResult<Character>(character);
    }

    /// <summary>
    /// Remove a character from the world
    /// </summary>
    /// <param name="world_ID"></param>
    /// <param name="character_ID"></param>
    /// <returns>True if successfull</returns>
    public ServiceResult<bool> RemoveCharacter(Guid world_ID, Guid character_ID)
    {
        var world = _db.Set<World>().Include(w=>w.Characters).FirstOrDefault(w=>w.ID == world_ID);
        if (world == null) throw new KeyNotFoundException(ErrorMessages.WORLD_NOT_EXIST);
        var character = world.Characters.FirstOrDefault(c=>c.ID == character_ID);
        if(character == null) throw new KeyNotFoundException(ErrorMessages.CHARACTER_NOT_EXIST);
        world.Characters.Remove(character);
        _db.SaveChanges();

        return ServiceResult<bool>.NoContent();
    }
}