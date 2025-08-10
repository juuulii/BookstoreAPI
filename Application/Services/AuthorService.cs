using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _authorRepository.GetAllAsync();
    }

    public async Task<Author> GetByIdAsync(int id)
    {
        return await _authorRepository.GetByIdAsync(id);
    }

    public async Task<Author> CreateAsync(Author author)
    {
        return await _authorRepository.AddAsync(author);
    }

    public async Task<bool> UpdateAsync(Author author)
    {
        return await _authorRepository.UpdateAsync(author);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _authorRepository.DeleteAsync(id);
    }
}
