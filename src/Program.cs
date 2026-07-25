using Microsoft.EntityFrameworkCore;
using Yggdrasil.Data;
using Yggdrasil.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Yggdrasil - AI Scenarios",
        Version = "v1",
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
    options.RootDirectory = "/src/Pages";
});



var app = builder.Build();
app.MapEndpoints();

app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/test", () => Results.Ok("works"));
app.Run();