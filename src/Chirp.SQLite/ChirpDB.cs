using Microsoft.Data.Sqlite;

namespace Chirp.SQLite;

public class ChirpDB : IDisposable
{
    private readonly SqliteConnection _connection;
    private bool _disposed;

    public ChirpDB()
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "chirp.db");
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
                Timestamp INTEGER NOT NULL
            )";
        command.ExecuteNonQuery();
    }

    public void AddCheep(Cheep cheep)
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Cheeps (Author, Message, Timestamp)
            VALUES ($author, $message, $timestamp)";
        command.Parameters.AddWithValue("$author", cheep.Author);
        command.Parameters.AddWithValue("$message", cheep.Message);
        command.Parameters.AddWithValue("$timestamp", cheep.Timestamp);
        command.ExecuteNonQuery();
    }

    public IEnumerable<Cheep> GetCheeps()
    {
        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT Author, Message, Timestamp
            FROM Cheeps
            ORDER BY Timestamp DESC";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new Cheep(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetInt64(2)
            );
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _connection.Dispose();
            _disposed = true;
        }
    }
}