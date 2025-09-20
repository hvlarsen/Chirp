using Chirp.SimpleDB;
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/health", () =>
{
    Results.Ok(new {status = "ok"});
});


app.MapGet("/cheeps", (int? limit) =>
{
    try
    {
        var cheeps = CsvDatabase<Cheep>.Instance.Read(limit).ToList();
        return Results.Ok(cheeps);
    }
    catch (FileNotFoundException)
    {
        return Results.Ok(new List<Cheep>());
    }
    
});


app.MapPost("/cheep", (Cheep cheep) =>
{
    CsvDatabase<Cheep>.Instance.Store(cheep);
    return Results.Ok();
});

app.Run();