namespace Chirp.Razor.Data;
public class CheepService : ICheepService
{
    private readonly DBFacade _dbFacade;
    private const int pageSize = 32; 

    public CheepService(DBFacade dbFacade)
    {
        _dbFacade = dbFacade;
    }
    public List<Cheep> GetCheeps(int page)
    {
        if (page < 1) page = 1;
        int skipPages = (page - 1) * pageSize;

        var cheeps = _dbFacade.GetCheeps();
        return cheeps.Skip(skipPages).Take(pageSize).ToList();
    }

    public List<Cheep> GetCheepsByAuthor(string author, int page)
    {
        if (page < 1) page = 1;
        int skipPages = (page - 1) * pageSize;

        var cheeps = _dbFacade.GetCheepsByAuthor(author);
        return cheeps.Skip(skipPages).Take(pageSize).ToList();
    }

    public void CreateCheep(string message, string author)
    {
        //Not implemented yet
    }
}