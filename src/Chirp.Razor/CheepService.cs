using Chirp.SimpleDB;

public interface ICheepService
{
    public List<Cheep> GetCheeps();
    public List<Cheep> GetCheepsFromAuthor(string author);
}

// CheepService.cs
public class CheepService : ICheepService
{
    private readonly DBFacade _dbFacade;

    public CheepService()
    {
        _dbFacade = new DBFacade();
    }

    public IEnumerable<Cheep> GetCheeps()
    {
        return _dbFacade.GetAllCheeps();
    }

    public IEnumerable<Cheep> GetCheepsFromAuthor(string author)
    {
        return _dbFacade.GetCheepsByAuthor(author);
    }

    public void CreateCheep(string message, string author)
    {
        var cheep = new Cheep
        {
            Message = message,
            Author = author,
            Timestamp = DateTime.UtcNow
        };
        _dbFacade.AddCheep(cheep);
    }

    public void Dispose()
    {
        _dbFacade.Dispose();
    }
}