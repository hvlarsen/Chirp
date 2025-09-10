using CsvHelper;
using System.Globalization;

namespace SimpleDB;

public sealed class CsvDatabase<T> : IDatabaseRepository<T>
{
    private readonly string _filePath = "chirp_cli_db.csv";
    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(_filePath);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        var messagesOut = csvReader.GetRecords<T>().ToList();
        return  messagesOut;
    }
    public void Store(T record)
    {
        var existingMessages = File.Exists(_filePath)
        ? this.Read().ToList()
        : new List<T>();

        existingMessages.Add(record);

        using var writer = new StreamWriter(_filePath, false);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(existingMessages);
        
    }
}