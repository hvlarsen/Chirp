namespace Chirp.Domain.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<Cheep> Cheeps { get; set; } = new();
}