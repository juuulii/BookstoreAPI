using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPublisherRepository
{
    Task<IEnumerable<Publisher>> GetAllAsync();
    Task<Publisher> GetByIdAsync(int id);
    Task<Publisher> AddAsync(Publisher publisher);
    Task<bool> UpdateAsync(Publisher publisher);
    Task<bool> DeleteAsync(int id);
}
