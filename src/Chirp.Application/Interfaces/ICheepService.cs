using Chirp.Domain.Entities;

namespace Chirp.Application.Interfaces;

public interface ICheepService
{
    public List<Cheep> GetCheeps(int page);
    public List<Cheep> GetCheepsByAuthor(string author, int page);
}