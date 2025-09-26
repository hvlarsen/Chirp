using Chirp.SimpleDB;

public interface ICheepService
{
    public List<Cheep> GetCheeps();
    public List<Cheep> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<Cheep> _cheeps = CsvDatabase<Cheep>.Instance.Read(100).ToList();

    public List<Cheep> GetCheeps()
    {
        return _cheeps;
    }

    public List<Cheep> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
