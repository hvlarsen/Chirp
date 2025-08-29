using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        using (var reader = new StreamReader("chirp_cli_db.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var messages = csv.GetRecords<ChirpOutput>();
            foreach (var message in messages)
            {
                Console.WriteLine($"{message.Author}: {message.Message} ({message.Timestamp})");
            }
        }
    }
}



public class ChirpOutput
{
    public string Author { get; set; }
    public string Message { get; set; }
    public int Timestamp { get; set; }
}