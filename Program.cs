using System.CommandLine;
using System.CommandLine.Invocation;
using System.Globalization;
using CsvHelper;

namespace Chirp.CLI;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var filepath = "chirp_cli_db.csv";

        var rootCommand = new RootCommand("Chirp (X formally known as Twitter) ");

        var readCommand = new Command("read", "Show all cheeps");
        readCommand.SetHandler(() =>
        {
            if (!File.Exists(filepath))
            {
                Console.WriteLine("No cheeps found.");
                return;
            }

            using var reader = new StreamReader(filepath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var messagesOut = csvReader.GetRecords<Cheep>();
            foreach (var message in messagesOut)
            {
                var dateFormatted = DateTimeOffset.FromUnixTimeSeconds(message.Timestamp).UtcDateTime;
                Console.WriteLine($"{message.Author} @ {dateFormatted} @ {message.Message}");
            }
        });

        var cheepCommand = new Command("cheep", "Add a new cheep");
        var messageArg = new Argument<string>("message", "Message to cheep");
        cheepCommand.AddArgument(messageArg);
        cheepCommand.SetHandler((string message) =>
        {
            var messagesIn = new List<Cheep>();
            if (File.Exists(filepath))
            {
                using var reader = new StreamReader(filepath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                messagesIn.AddRange(csv.GetRecords<Cheep>());
            }

            string currentUser = Environment.UserName;
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var cheep = new Cheep(currentUser, message, currentTimestamp);
            messagesIn.Add(cheep);

            using var writer = new StreamWriter(filepath, false);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(messagesIn);

            Console.WriteLine("Cheep added!");
        }, messageArg);

        rootCommand.AddCommand(readCommand);
        rootCommand.AddCommand(cheepCommand);

        return await rootCommand.InvokeAsync(args);
    }

    public record Cheep(string Author, string Message, long Timestamp);
}
