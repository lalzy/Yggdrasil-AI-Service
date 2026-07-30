using Yggdrasil.Data;
using Yggdrasil.DTO;
using Yggdrasil.Util;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Models;
using yggdrasil.Util;


namespace Yggdrasil.Services;

public class CharacterService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public ServiceResult<List<CharacterSummary>> GetAll(int? count=null)
    {
        
        var query = _db.Set<Character>().Where<Character>(c=>c.Name != null && c.Description != null).Select(c=> new CharacterSummary(c.ID, c.Name, c.Description));
        if (count.HasValue)
        {
            if(count < 1) throw new ArgumentException(ErrorMessages.LESSTHANONE);
            query = query.Take(count.Value);
        }

        return new (query.ToList());
    }

    public ServiceResult<Character> GetOne(Guid character_ID)
    {
        var world = _db.Set<Character>().FirstOrDefault(c=>c.ID == character_ID);
        if(world == null) throw new KeyNotFoundException(ErrorMessages.WORLD_NOT_EXIST);
        return new (world);
    }

    public ServiceResult<Character> Create(CharacterRequest request)
    {
        var character = request.ConvertModelToDTO<Character>();

        _db.Set<Character>().Add(character);
        _db.SaveChanges();
        return new ServiceResult<Character>(character);
    }

    public ServiceResult<bool> Delete(Guid character_ID)
    {
        var rows = _db.Set<Character>().Where(c=>c.ID == character_ID).ExecuteDelete();
        if(rows == 0) throw new KeyNotFoundException(ErrorMessages.CHARACTER_NOT_EXIST);
        return new ServiceResult<bool>(true);
    }
}
