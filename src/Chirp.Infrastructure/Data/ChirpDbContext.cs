using Chirp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Data;

public class ChirpDbContext : DbContext
{
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options) : base(options) { }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
}