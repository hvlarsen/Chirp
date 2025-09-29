using Chirp.SimpleDB;

public interface ICheepService
{
    public List<Cheep> GetCheeps(int page);
    public List<Cheep> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    private const int pageSize = 32; 

    public List<Cheep> GetCheeps(int page)
    {
        if (page < 1) page = 1;

        int skipPages = (page - 1) * pageSize;

        Console.WriteLine($"[CheepService] Reading cheeps from: {Path.GetFullPath("chirp_cli_db.csv")} for page {page}");
        return CsvDatabase<Cheep>.Instance.Read().Skip(skipPages).Take(pageSize).ToList();
    }

    public List<Cheep> GetCheepsFromAuthor(string author, int page)
    {
        if (page < 1) page = 1;

        int skipPages = (page - 1) * pageSize;

        Console.WriteLine($"[CheepService] Reading cheeps for author {author} from: {Path.GetFullPath("chirp_cli_db.csv")} for page {page}");
        return CsvDatabase<Cheep>.Instance.Read().Where(x => x.Author == author).Skip(skipPages).Take(pageSize).ToList();
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}