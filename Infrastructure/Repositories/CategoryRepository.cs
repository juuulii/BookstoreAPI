using Domain.Entities;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationContext _context;

    public CategoryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(bool includeDeleted = false)
    {
        var query = _context.Categories
            .Include(c => c.Books)
                .ThenInclude(b => b.Author)
            .Include(c => c.Books)
                .ThenInclude(b => b.Publisher)
            .AsQueryable();

        if (!includeDeleted)
            query = query.Where(c => !c.IsDeleted);

        return await query.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        var query = _context.Categories
            .Include(c => c.Books)
                .ThenInclude(b => b.Author)
            .Include(p => p.Books)
                .ThenInclude(b => b.Publisher)
            .AsQueryable();

        if (!includeDeleted)
            query = query.Where(c => !c.IsDeleted);

        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        category.IsDeleted = true; 
        _context.Categories.Update(category);

        return await _context.SaveChangesAsync() > 0;
    }
}
