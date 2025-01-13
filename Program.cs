using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

var app = builder.Build();

app.UseStaticFiles();
app.UseCors(option => option.AllowAnyOrigin());

app.MapGet("/", async context => { context.Response.Redirect("/login.html"); await Task.CompletedTask; });

app.MapPost("/login", async context =>
{
  using var reader = new StreamReader(context.Request.Body);
  var body = await reader.ReadToEndAsync();
  var data = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
  var user = data["user"];
  var password = data["password"];

  context.Response.StatusCode = user == "admin" && password == "admin"
    ? StatusCodes.Status200OK
    : StatusCodes.Status401Unauthorized;
});

app.Run();
