using Domain.Entities;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AuthorRepository : IAuthorRepository
{
    private readonly ApplicationContext _context;

    public AuthorRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors
            .Include(a => a.Books)
            .ToListAsync();
    }

    public async Task<Author> GetByIdAsync(int id)
    {
        return await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Author> AddAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<bool> UpdateAsync(Author author)
    {
        _context.Authors.Update(author);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
            return false;

        _context.Authors.Remove(author);
        return await _context.SaveChangesAsync() > 0;
    }
}
