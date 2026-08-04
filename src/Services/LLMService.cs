// LLMService.cs

using System.Xml.Linq;
using Yggdrasil.Data;
using Yggdrasil.DTO;
using Yggdrasil.Util;
using Yggdrasil.Models;


namespace Yggdrasil.Services;

public class LLMService(AppDbContext db){
    private readonly AppDbContext _db = db;

    private string CreateBaseCharacterString(CharacterBase character){
        var lines = new[]{
            $"FullName: {character.FullName}",
            $"Gender: {character.Gender}",
            (character.Pronouns != null ? $"Pronouns: {character.Pronouns}" : null),
            (character.Race != null ? $"Race: {character.Race}" : null),
            (character.Occupation != null ? $"Occupation: {character.Occupation}" : null),
            (character.Appearance != null ? $"Appearance: {character.Appearance}" : null),
            (character.Equipment != null ? $"Equipment: {character.Equipment}" : null)
        };

        return string.Join("\n", lines.Where(l => l != null));
    }

    private string createCharacterString(Character character){
        var lines = new[]{
            (character.Personality != null ? $"Pronouns: {character.Personality}" : null),
        };
        
        return string.Concat(CreateBaseCharacterString(character), "\n", string.Join("\n", lines.Where(l => l != null)));
    }

    private string CreateSystemPrompt(World world, Persona persona){
        var userElement = new XElement("user", new XText("\n"+CreateBaseCharacterString(persona)));
        userElement.Add(new XAttribute(XNamespace.None + "name", persona.Name));
        var charactersElement = new XElement("characters");

        charactersElement.Add(world.Characters.Select(c =>
        {
            var characterElement = new XElement("char", new XText("\n"+createCharacterString(c)));
            characterElement.Add(new XAttribute(XNamespace.None + "name", c.Name));
            return characterElement;
        }));

        var prompt = new XDocument(
            new XElement("system", XElement.Parse(world.NarratorInstruction!),
                new XElement("world", 
                    new XElement("scenario", world.Scenario),
                        userElement, charactersElement)));

        return prompt.ToString(SaveOptions.None);
    }

    private object createLLMPayload(World world, Persona persona){
        var payload = new LLMPayload();

        payload.Messages.Add(new Message { Role = "system", Content = CreateSystemPrompt(world, persona) });

        return payload;
    }
}
