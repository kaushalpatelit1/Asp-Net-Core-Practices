using ChatService.Hubs;
using ChatService.Users;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(bldr =>
    {
        bldr.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());

var app = builder.Build();

app.MapGet("/", () => "ChatHub server is Running.");
app.UseRouting();
app.UseCors();

app.UseEndpoints(endPoints =>
{
    endPoints.MapHub<ChatHub>("/chat");
});

app.Run();
