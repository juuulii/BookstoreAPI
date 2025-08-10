using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author> GetByIdAsync(int id);
    Task<Author> AddAsync(Author author);
    Task<bool> UpdateAsync(Author author);
    Task<bool> DeleteAsync(int id);
}

