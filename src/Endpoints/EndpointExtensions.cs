using System.Reflection;

namespace AIService.Endpoints;

public static class EndpointExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var methods = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t.Namespace == "AIService.Endpoints")
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(m => m.Name.StartsWith("Map") && m != typeof(EndpointExtensions).GetMethod("MapEndpoints"));

        foreach(var method in methods)
        {
            method.Invoke(null, new object[] {app});
        }
    }
}