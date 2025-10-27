namespace Chirp.Domain.Entities;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Optional link to Identity User
    public string? ApplicationUserId { get; set; }  // FK to AspNetUsers table
    public ApplicationUser? ApplicationUser { get; set; }  // Navigation property
    
    public List<Cheep> Cheeps { get; set; } = new();
}