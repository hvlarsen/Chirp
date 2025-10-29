using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Domain.Entities;

public class ApplicationUser: IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string DisplayName { get; set; } = string.Empty;

    // Navigation to Author (if used)
    public Author? Author { get; set; }
}