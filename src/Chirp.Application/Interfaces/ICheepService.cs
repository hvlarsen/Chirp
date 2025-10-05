namespace Chirp.Razor.Data;

public interface ICheepService
{
    public List<Cheep> GetCheeps(int page);
    public List<Cheep> GetCheepsByAuthor(string author, int page);
}