using Chirp.Application.DTOs;

namespace Chirp.Application.Interfaces;

public interface ICheepService
{
    Task<List<CheepDto>> GetPublicTimelineAsync(int page);
    Task<List<CheepDto>> GetPrivateTimelineAsync(string author, int page);
}