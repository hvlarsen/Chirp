using Microsoft.Data.Sqlite;

namespace Chirp.Razor.Data;

public class DBFacade
{
    private readonly string _dbpath;

    public DBFacade()
    {
        var envPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");

        if (!string.IsNullOrEmpty(envPath))
        {
            _dbpath = envPath;
        } else {
            var tempDir = Path.GetTempPath();
            _dbpath = Path.Combine(tempDir, "chirp.db");
        }
    }
}