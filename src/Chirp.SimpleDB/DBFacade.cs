namespace Chirp.SimpleDB;

// DBFacade.cs
using Microsoft.Data.Sqlite;

public class DBFacade : IDisposable
{
    private readonly SqliteConnection _connection;

    public DBFacade()
    {
        var dbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH") 
            ?? throw new InvalidOperationException("CHIRPDBPATH environment variable not set");
        _connection = new SqliteConnection($"Data Source={dbPath}");
        _connection.Open();
        CreateTables();
    }

    private void CreateTables()
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Cheeps (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Author TEXT NOT NULL,
                Message TEXT NOT NULL,
                Timestamp TEXT NOT NULL
            )";
        command.ExecuteNonQuery();
    }

    public IEnumerable<Cheep> GetAllCheeps()
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT Author, Message, Timestamp 
            FROM Cheeps 
            ORDER BY Timestamp DESC";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new Cheep
            {
                Author = reader.GetString(0),
                Message = reader.GetString(1),
                Timestamp = DateTime.Parse(reader.GetString(2))
            };
        }
    }

    public IEnumerable<Cheep> GetCheepsByAuthor(string author)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT Author, Message, Timestamp 
            FROM Cheeps 
            WHERE Author = $author 
            ORDER BY Timestamp DESC";
        command.Parameters.AddWithValue("$author", author);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new Cheep
            {
                Author = reader.GetString(0),
                Message = reader.GetString(1),
                Timestamp = DateTime.Parse(reader.GetString(2))
            };
        }
    }

    public void AddCheep(Cheep cheep)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Cheeps (Author, Message, Timestamp)
            VALUES ($author, $message, $timestamp)";
        command.Parameters.AddWithValue("$author", cheep.Author);
        command.Parameters.AddWithValue("$message", cheep.Message);
        command.Parameters.AddWithValue("$timestamp", cheep.Timestamp.ToString("o"));
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}