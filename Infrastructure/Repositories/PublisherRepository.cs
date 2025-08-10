using Domain.Entities;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PublisherRepository : IPublisherRepository
{
    private readonly ApplicationContext _context;

    public PublisherRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Publisher>> GetAllAsync()
    {
        return await _context.Publishers
            .Include(p => p.Books)
            .ToListAsync();
    }

    public async Task<Publisher> GetByIdAsync(int id)
    {
        return await _context.Publishers
            .Include(p => p.Books)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Publisher> AddAsync(Publisher publisher)
    {
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();
        return publisher;
    }

    public async Task<bool> UpdateAsync(Publisher publisher)
    {
        _context.Publishers.Update(publisher);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher == null) return false;

        _context.Publishers.Remove(publisher);
        return await _context.SaveChangesAsync() > 0;
    }
}

