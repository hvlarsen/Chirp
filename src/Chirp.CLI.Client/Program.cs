using System.CommandLine;
using System.Net.Http.Json;
using Chirp.SQLite; // Only because it needs to know Cheep.cs

namespace Chirp.CLI;

public class Program
{
    private static HttpClient _http = new();
    public static void UseHttpClient(HttpClient client) => _http = client; // Method used only for tests. Overwrites _http
    public static Task<int> Main(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        var baseUrl = Environment.GetEnvironmentVariable("CHIRP_SERVICE_URL") ??
        (env == "Development" ? "http://localhost:5165" : "https://bdsagroup19chirprazor.azurewebsites.net");
        _http.BaseAddress = new Uri(baseUrl);

        var rootCommand = new RootCommand("Chirp (X formally known as Twitter) ");

        var readCommand = new Command("read", "Show all cheeps");
        readCommand.SetHandler(async () =>
        {
            var cheeps = await _http.GetFromJsonAsync<List<Cheep>>("/cheeps");
            if (cheeps is not null)
                UI.PrintCheeps(cheeps);
        });

        var cheepCommand = new Command("cheep", "Add a new cheep");
        var messageArg = new Argument<string>("message", "Message to cheep");
        cheepCommand.AddArgument(messageArg);
        cheepCommand.SetHandler(async (message) =>
        {
            string currentUser = Environment.UserName;
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var cheep = new Cheep(currentUser, message, currentTimestamp);
            var response = await _http.PostAsJsonAsync("/cheep", cheep);
            Console.WriteLine("Cheep added!");
        }, messageArg);

        rootCommand.AddCommand(readCommand);
        rootCommand.AddCommand(cheepCommand);

        return rootCommand.InvokeAsync(args); }
}