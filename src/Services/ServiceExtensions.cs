// ServiceExtensions.cs
using Yggdrasil.Services;
using System.Reflection;

/// <summary>Register the services automatically that exist in the Yggdrasil.Services namespace</summary>
public static class ServiceExtensions{
    public static void AddServices(this IServiceCollection services){
        var serviceTypes = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract 
            && t.Namespace == "Yggdrasil.Services"
            && !t.IsSubclassOf(typeof(DbContext))
            && !t.IsNested);

        foreach (var type in serviceTypes){
            if(type == typeof(ChatService)) services.AddHttpClient<ChatService>();
            else services.AddScoped(type);
        }
    }
}
