// PayloadFactory.cs

using Yggdrasil.DTO;

namespace Yggdrasil.Tests.Factories;

public class LLMPayloadFactory{
    public static LLMPayload Create(List<Message>? messages = null){
        var faker = new Faker();
        var payload = new LLMPayload();

        if(messages == null){
            messages = [];
            messages.Add(new Message{Role="System", Content=faker.Lorem.Lines()});
            messages.Add(new Message { Role = "user", Content = "" });
            messages.Add(new Message { Role = "assisstant", Content = faker.Lorem.Lines() });
            messages.Add(new Message { Role = "user", Content = faker.Lorem.Lines() });
        }
        payload.Messages = messages;
        return payload;
    }
}
