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

    public List<Cheep> getCheeps()
    {
        
    }
}