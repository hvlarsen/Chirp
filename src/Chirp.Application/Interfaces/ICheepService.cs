namespace Chirp.Application.Interfaces;

using Chirp.Domain.Entities;

public interface ICheepService
{
    public List<Cheep> GetCheeps(int page);
    public List<Cheep> GetCheepsByAuthor(string author, int page);
}