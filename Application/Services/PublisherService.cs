using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PublisherService
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherService(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public async Task<IEnumerable<Publisher>> GetAllAsync(bool includeDeleted = false)
    {
        return await _publisherRepository.GetAllAsync(includeDeleted);
    }

    public async Task<Publisher?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        return await _publisherRepository.GetByIdAsync(id, includeDeleted);
    }

    public async Task<Publisher> CreateAsync(Publisher publisher)
    {
        return await _publisherRepository.AddAsync(publisher);
    }

    public async Task<bool> UpdateAsync(Publisher publisher)
    {
        return await _publisherRepository.UpdateAsync(publisher);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _publisherRepository.DeleteAsync(id);
    }
}
