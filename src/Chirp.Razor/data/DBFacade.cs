using Microsoft.Data.Sqlite;

namespace Chirp.Razor.Data;

public class DBFacade
{
    private readonly string _dbPath;
    private readonly SqliteConnection _connection;

    public DBFacade()
    {
        var envPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if (!string.IsNullOrEmpty(envPath))
        {
            _dbPath = envPath;
        }
        else
        {
            var tempDir = Path.GetTempPath();
            _dbPath = Path.Combine(tempDir, "chirp.db");
        }

        _connection = new SqliteConnection($"Data Source={_dbPath}");
    }

    public List<Cheep> GetCheeps(int limit = 32)
    {
        _connection.Open();
        var cheeps = new List<Cheep>();
        var command = _connection.CreateCommand();
        command.CommandText = $@"select pub_date, message_id, author_id, text from message limit {limit}";

        var reader = command.ExecuteReader();

        while (reader.Read()) {
            int pubDate = reader.GetInt32(0);
            int messageId = reader.GetInt32(1);
            int authorId = reader.GetInt32(2);
            string text = reader.GetString(3);

            cheeps.Add(new Cheep(messageId, authorId, text, pubDate));
        }
        return cheeps;
    }
}