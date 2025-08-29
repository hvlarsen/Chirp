using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        using var reader = new StreamReader("chirp_cli_db.csv");
        using var csv1 = new CsvReader(reader, CultureInfo.InvariantCulture);
        using var writer = new StreamWriter("chirp_cli_db.csv");
        using var csv2 = new CsvWriter(writer, CultureInfo.InvariantCulture);
        if (args[0] == "read")
        { 
            var messagesOut = csv1.GetRecords<ChirpOutput>(); 
            foreach (var message in messagesOut)
            { 
                Console.WriteLine($"{message.Author}: {message.Message} ({message.Timestamp})");
            }
        }
        else if (args[0] == "cheep")
        {
            var messagesIn = new List<Cheep>
            {
                new Cheep { Author = "TestName", Message = args[1], Timestamp = 12 }
            };
            csv2.WriteRecords(messagesIn);
        }
    }
}



public class ChirpOutput
{
    public required string Author { get; set; }
    public required string Message { get; set; }
    public int Timestamp { get; set; }
}

public class Cheep
{    
    public required string Author { get; set; }
    public required string Message { get; set; }
    public int Timestamp { get; set; }
}