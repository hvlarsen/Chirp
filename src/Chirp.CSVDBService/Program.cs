using Chirp.SimpleDB;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5165");

var app = builder.Build();

app.MapGet("/cheeps", (int? limit) =>
{
    var cheeps = CsvDatabase<Cheep>.Instance.Read(limit).ToList();
    return Results.Ok(cheeps);
});

app.MapPost("/cheep", (Cheep cheep) =>
{
    CsvDatabase<Cheep>.Instance.Store(cheep);
    return Results.Ok();
});

app.Run();