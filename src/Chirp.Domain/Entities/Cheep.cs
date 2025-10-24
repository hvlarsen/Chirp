using System.ComponentModel.DataAnnotations;

namespace Chirp.Domain.Entities;

public class Cheep
{
    public int CheepId { get; set; }
    [StringLength(500)]
    public string Text { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    

    //Foreign keys:
    public int AuthorId { get; set; }

    public required Author Author { get; set; }
}