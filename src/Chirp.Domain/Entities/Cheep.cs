namespace Chirp.Domain.Entities;

public class Cheep
{
    public int CheepId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}