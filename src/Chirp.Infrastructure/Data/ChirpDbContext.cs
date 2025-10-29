using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Chirp.Domain.Entities;

namespace Chirp.Infrastructure.Data;

public class ChirpDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Cheep> Cheeps { get; set; } = null!;

    public ChirpDbContext(DbContextOptions<ChirpDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>()
            .HasMany(a => a.Cheeps)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Author)
            .WithOne(a => a.ApplicationUser)
            .HasForeignKey<Author>(a => a.ApplicationUserId);
    }
}