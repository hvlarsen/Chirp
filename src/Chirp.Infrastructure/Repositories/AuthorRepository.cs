using Chirp.Domain.Entities;
using Chirp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Chirp.Infrastructure.Repositories;

public class AuthorRepository
{
    private readonly ChirpDbContext _context;

    public AuthorRepository(ChirpDbContext context)
    {
        _context = context;
    }
    
    public async Task<Author?> GetAuthorByName(string name)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Name == name);
    }
    
    public async Task<Author?> GetAuthorByEmail(string email)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Email == email);
    }
    
    public async Task CreateAuthorAsync(string name, string email)
    {
        bool nameExists = await _context.Authors.AnyAsync(a => a.Name == name);
        bool emailExists = await _context.Authors.AnyAsync(a => a.Email == email);

        if (nameExists || emailExists)
        {
            throw new InvalidOperationException(
                $"An author with the same {(nameExists ? "name" : "email")} already exists.");
        }

        var newAuthor = new Author
        {
            Name = name,
            Email = email
        };

        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();
    }
}